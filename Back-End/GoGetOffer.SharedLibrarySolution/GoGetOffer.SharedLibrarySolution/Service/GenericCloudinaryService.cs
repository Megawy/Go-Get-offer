using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using GoGetOffer.SharedLibrarySolution.DTOs.Cloudinary;
using GoGetOffer.SharedLibrarySolution.Interface;
using GoGetOffer.SharedLibrarySolution.Responses;
using GoGetOffer.SharedLibraySolution.DTOs.Cloudinary;
using Microsoft.Extensions.Options;
using static GoGetOffer.SharedLibrarySolution.Interface.IGenericCloudinary;

namespace GoGetOffer.SharedLibrarySolution.Service
{
    public class GenericCloudinaryService : IGenericCloudinary
    {
        private readonly Cloudinary _cloudinary;
        private readonly CloudinaryOptions _options;

        public GenericCloudinaryService(IOptions<CloudinaryOptions> options)
        {
            _options = options.Value;
            var account = new Account(
                _options.CloudName,
                _options.ApiKey,
                _options.ApiSecret
                );
            _cloudinary = new Cloudinary(account);
            _cloudinary.Api.Secure = true;
        }

        public async Task<Response<MediaInfoDTO>> UploadAsync(UploadRequestDTO request, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (request.FileStream == null)
                return Response<MediaInfoDTO>.Failure("No file stream provided.");

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(request.FileName ?? $"file_{Guid.NewGuid():N}", request.FileStream),
                Folder = request.Folder ?? _options.DefaultFolder,
                Overwrite = request.Overwrite,
                PublicId = request.PublicId,
                Transformation = ToTransformation(request.Transform),
                Tags = request.Tags != null ? string.Join(",", request.Tags) : null
            };

            var result = await _cloudinary.UploadAsync(uploadParams);

            if (result.Error != null)
                return Response<MediaInfoDTO>.Failure($"Upload failed: {result.Error.Message}");

            var dto = new MediaInfoDTO
            {
                PublicId = result.PublicId!,
                Url = result.Url?.ToString() ?? string.Empty,
                SecureUrl = result.SecureUrl?.ToString() ?? string.Empty,
                ResourceType = result.ResourceType!,
                Format = result.Format!,
                Bytes = result.Bytes,
                Width = result.Width,
                Height = result.Height,
                CreatedAt = result.CreatedAt,
                Tags = result.Tags ?? Array.Empty<string>(),
                Version = result.Version?.ToString() ?? string.Empty
            };

            return Response<MediaInfoDTO>.Success(dto, "Upload successful");
        }

        public async Task<Response<MediaInfoDTO>> DeleteAsync(string publicId, MediaType type = MediaType.Image, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var deletionParams = new DeletionParams(publicId)
            {
                ResourceType = type switch
                {
                    MediaType.Image => ResourceType.Image,
                    MediaType.Video => ResourceType.Video,
                    _ => ResourceType.Raw
                }
            };

            var result = await _cloudinary.DestroyAsync(deletionParams);

            bool success = result.Result == "ok" || result.Result == "not found";

            if (success)
            {
                return Response<MediaInfoDTO>.Success($"Deleted: {result.Result}");
            }

            return Response<MediaInfoDTO>.Failure($"Delete failed: {result.Result}");
        }

        public string BuildUrl(string publicId, TransformOptionsDTO? transform = null, MediaType type = MediaType.Image)
        {
            var url = type switch
            {
                MediaType.Image => _cloudinary.Api.UrlImgUp,
                MediaType.Video => _cloudinary.Api.UrlVideoUp,
                _ => _cloudinary.Api.UrlImgUp.ResourceType("raw")
            };

            var transformation = ToTransformation(transform);
            return url.Transform(transformation).BuildUrl(publicId);
        }

        private static Transformation ToTransformation(TransformOptionsDTO? t)
        {
            var tr = new Transformation();
            if (t == null) return tr;

            if (t.Width.HasValue) tr = tr.Width(t.Width.Value);
            if (t.Height.HasValue) tr = tr.Height(t.Height.Value);
            if (!string.IsNullOrWhiteSpace(t.Crop)) tr = tr.Crop(t.Crop);
            if (!string.IsNullOrWhiteSpace(t.Gravity)) tr = tr.Gravity(t.Gravity);
            if (!string.IsNullOrWhiteSpace(t.Background)) tr = tr.Background(t.Background);
            if (!string.IsNullOrWhiteSpace(t.Effect)) tr = tr.Effect(t.Effect);
            if (!string.IsNullOrWhiteSpace(t.Quality)) tr = tr.Quality(t.Quality);

            return tr;
        }
    }
}
