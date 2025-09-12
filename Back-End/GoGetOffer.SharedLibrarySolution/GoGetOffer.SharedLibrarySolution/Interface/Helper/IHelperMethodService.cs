using GoGetOffer.SharedLibrarySolution.DTOs;
using GoGetOffer.SharedLibrarySolution.Responses;

namespace GoGetOffer.SharedLibrarySolution.Interface.Helper
{
    public interface IHelperMethodService
    {
        bool IsValidGuid(Guid? guidValue);
        string Normalize(string? value);
        List<string> EncryptListSafe(List<string>? list);
        string EncryptStringSafe(string? value);
        string DecryptStringSafe(string? value);
        List<string> DecryptListSafe(List<string>? list);
        Task<Response<string>> CheckAndCache(string operation, string key, int maxAttempts, TimeSpan timeWindow);
        Response<ImgDTO> DecryptImage(ImgDTO data);
    }
}
