using GoGetOffer.SharedLibrarySolution.DTOs;
using GoGetOffer.SharedLibrarySolution.Interface;
using GoGetOffer.SharedLibrarySolution.Interface.Helper;
using GoGetOffer.SharedLibrarySolution.Responses;
using GoGetOffer.SharedLibrarySolution.Service.Helper;
using GoGetOffer.SharedLibraySolution.DTOs.Cloudinary;
using Microsoft.AspNetCore.Http;

namespace GoGetOffer.SharedLibrarySolution.Service
{
    public class ValidateUploadAndEncrypt(IGenericCloudinary cloudinary,
        IAesEncryptionHelperService aesEncryptionHelperService) : IValidateUploadAndEncrypt
    {
        private readonly IGenericCloudinary _cloudinary = cloudinary;
        private readonly IAesEncryptionHelperService _aesEncryptionHelperService = aesEncryptionHelperService;

        private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase) { ".jpg", ".jpeg", ".png", ".pdf", ".svg" };

        public async Task<(bool ok, string? error, List<(string url, string publicId)> items)> ValidateUploadAndEncryptAsync(IEnumerable<IFormFile> files, string folder)
        {
            var results = new List<(string url, string publicId)>();
            foreach (var file in files)
            {
                if (file is null || file.Length <= 0)
                    return (false, $"File {file?.FileName ?? "Unnamed"} is empty.", results);

                if (file.Length > FileValidationSettings.MaxFileSize)
                    return (false, $"File {file.FileName} exceeds max size.", results);

                var ext = Path.GetExtension(file.FileName);
                if (!AllowedExtensions.Contains(ext))
                    return (false, $"File {file.FileName} has invalid extension.", results);

                await using var stream = file.OpenReadStream();
                var uploadRequest = new UploadRequestDTO
                {
                    FileName = file.FileName,
                    FileStream = stream,
                    Folder = folder,
                    Type = MediaType.Image
                };
                var uploaded = await _cloudinary.UploadAsync(uploadRequest);
                if (!uploaded.Status) return (false, uploaded.Message!, results);

                var img = new ImgDTO { URL = uploaded.Data!.SecureUrl, PublicId = uploaded.Data.PublicId };
                var enc = CreateEncryptedImgs(img);
                if (!enc.Status) return (false, "Image encryption failed.", results);

                results.Add((enc.Data!.URL!, enc.Data!.PublicId!));
            }
            return (true, null, results);
        }

        public async Task<(bool ok, string? error, List<(string url, string publicId)> items)> ValidateUpload(IEnumerable<IFormFile> files, string folder)
        {
            var results = new List<(string url, string publicId)>();
            foreach (var file in files)
            {
                if (file is null || file.Length <= 0)
                    return (false, $"File {file?.FileName ?? "Unnamed"} is empty.", results);

                if (file.Length > FileValidationSettings.MaxFileSize)
                    return (false, $"File {file.FileName} exceeds max size.", results);

                var ext = Path.GetExtension(file.FileName);
                if (!AllowedExtensions.Contains(ext))
                    return (false, $"File {file.FileName} has invalid extension.", results);

                await using var stream = file.OpenReadStream();
                var uploadRequest = new UploadRequestDTO
                {
                    FileName = file.FileName,
                    FileStream = stream,
                    Folder = folder,
                    Type = MediaType.Image
                };
                var uploaded = await _cloudinary.UploadAsync(uploadRequest);
                if (!uploaded.Status) return (false, uploaded.Message!, results);

                var img = new ImgDTO { URL = uploaded.Data!.SecureUrl, PublicId = uploaded.Data.PublicId };

                results.Add((img.URL!, img.PublicId!));
            }
            return (true, null, results);
        }

        private Response<ImgDTO> CreateEncryptedImgs(ImgDTO dto)
        {

            var result = new ImgDTO
            {
                URL = EncryptStringSafe(dto.URL),
                PublicId = EncryptStringSafe(dto.PublicId),
            };
            return Response<ImgDTO>.Success(result, "Done");
        }

        private string EncryptStringSafe(string? value)
        {
            return _aesEncryptionHelperService.EncryptString(value ?? string.Empty);
        }

        public async Task<(bool ok, string? error, (string url, string publicId)? item)> ValidateUploadOneImg(IFormFile file, string folder)
        {
            if (file == null || file.Length <= 0)
                return (false, $"File {file?.FileName ?? "Unnamed"} is empty.", null);

            if (file.Length > FileValidationSettings.MaxFileSize)
                return (false, $"File {file.FileName} exceeds max size.", null);

            var ext = Path.GetExtension(file.FileName);
            if (!AllowedExtensions.Contains(ext))
                return (false, $"File {file.FileName} has invalid extension.", null);

            await using var stream = file.OpenReadStream();

            var uploadRequest = new UploadRequestDTO
            {
                FileName = file.FileName,
                FileStream = stream,
                Folder = folder,
                Type = MediaType.Image
            };

            var uploaded = await _cloudinary.UploadAsync(uploadRequest);
            if (!uploaded.Status) return (false, uploaded.Message!, null);

            var img = new ImgDTO { URL = uploaded.Data!.SecureUrl, PublicId = uploaded.Data.PublicId };

            return (true, null, (img!.URL!, img!.PublicId!));
        }
    }
}
