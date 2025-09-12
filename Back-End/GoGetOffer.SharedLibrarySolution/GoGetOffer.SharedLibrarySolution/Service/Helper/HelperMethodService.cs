using AutoMapper;
using GoGetOffer.SharedLibrarySolution.DTOs;
using GoGetOffer.SharedLibrarySolution.Interface.Helper;
using GoGetOffer.SharedLibrarySolution.Interface.Redis;
using GoGetOffer.SharedLibrarySolution.Responses;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GoGetOffer.SharedLibrarySolution.Service.Helper
{
    public class HelperMethodService(IAesEncryptionHelperService aesEncryptionHelperService,
        IGenericRedisService genericRedisService) : IHelperMethodService
    {
        private readonly IAesEncryptionHelperService _aesEncryptionHelperService = aesEncryptionHelperService;
        private readonly IGenericRedisService _genericRedisService = genericRedisService;

        public bool IsValidGuid(Guid? guidValue)
        {
            return guidValue.HasValue && guidValue.Value != Guid.Empty;
        }

        public string Normalize(string? value)
        {
            return value?.Trim().ToLower() ?? string.Empty;
        }

        public string DecryptStringSafe(string? value)
        {
            return _aesEncryptionHelperService.DecryptString(value ?? string.Empty);
        }

        public List<string> EncryptListSafe(List<string>? list)
        {
            return list?.Select(p => _aesEncryptionHelperService.DecryptString(p ?? string.Empty))
                       .ToList() ?? new List<string>();
        }

        public string EncryptStringSafe(string? value)
        {
            return _aesEncryptionHelperService.EncryptString(value ?? string.Empty);
        }

        public List<string> DecryptListSafe(List<string>? list)
        {
            return list?.Select(p => _aesEncryptionHelperService.DecryptString(p ?? string.Empty))
                       .ToList() ?? new List<string>();
        }

        public async Task<Response<string>> CheckAndCache(string operation, string key, int maxAttempts, TimeSpan timeWindow)
        {
            var rateLimitKey = $"CheckAndCache-{operation}:{key}";
            return await _genericRedisService.CheckAndCacheRequestAsync(rateLimitKey, maxAttempts, timeWindow);
        }

        public Response<ImgDTO> DecryptImage(ImgDTO data)
        {
            if (data == null)
                return Response<ImgDTO>.Failure("data is null");

            data.PublicId = DecryptStringSafe(data.PublicId);
            data.URL = DecryptStringSafe(data.URL);

            return Response<ImgDTO>.Success(data, $"Brance decrypted successfully at {DateTime.UtcNow}");
        }
    }
}
