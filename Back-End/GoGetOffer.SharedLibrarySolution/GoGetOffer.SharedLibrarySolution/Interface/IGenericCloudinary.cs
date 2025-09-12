using GoGetOffer.SharedLibrarySolution.DTOs.Cloudinary;
using GoGetOffer.SharedLibrarySolution.Responses;
using GoGetOffer.SharedLibraySolution.DTOs.Cloudinary;

namespace GoGetOffer.SharedLibrarySolution.Interface
{
    public interface IGenericCloudinary
    {
        public sealed class CloudinaryOptions
        {
            public string CloudName { get; init; } = string.Empty;
            public string ApiKey { get; init; } = string.Empty;
            public string ApiSecret { get; init; } = string.Empty;
            public bool UseSecureCdn { get; init; } = true; // https
            public string? DefaultFolder { get; init; } = "app/uploads"; // optional default folder
        }


        Task<Response<MediaInfoDTO>> UploadAsync(UploadRequestDTO request, CancellationToken cancellationToken = default);

        Task<Response<MediaInfoDTO>> DeleteAsync(string publicId, MediaType type = MediaType.Image, CancellationToken cancellationToken = default);

        string BuildUrl(string publicId, TransformOptionsDTO? transform = null, MediaType type = MediaType.Image);
    }
}
