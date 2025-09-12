using Microsoft.AspNetCore.Http;

namespace GoGetOffer.SharedLibrarySolution.Interface
{
    public interface IValidateUploadAndEncrypt
    {
        Task<(bool ok, string? error, List<(string url, string publicId)> items)> ValidateUpload(IEnumerable<IFormFile> files, string folder);
        Task<(bool ok, string? error, (string url, string publicId)? item)> ValidateUploadOneImg(IFormFile file, string folder);
        Task<(bool ok, string? error, List<(string url, string publicId)> items)> ValidateUploadAndEncryptAsync(IEnumerable<IFormFile> files, string folder);
    }
}
