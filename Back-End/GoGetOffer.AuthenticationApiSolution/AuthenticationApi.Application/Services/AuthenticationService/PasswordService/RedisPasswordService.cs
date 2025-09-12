using AuthenticationApi.Application.Services.Interfaces.AuthenticationService.PasswordService;
using GoGetOffer.SharedLibrarySolution.Interface.Helper;
using GoGetOffer.SharedLibrarySolution.Responses;
using StackExchange.Redis;

namespace AuthenticationApi.Application.Services.AuthenticationService.PasswordService
{
    public class RedisPasswordService
        (IConnectionMultiplexer redis,
        IAesEncryptionHelperService aesEncryptionHelperService) : IRedisPasswordService
    {
        private readonly IDatabase _redis = redis.GetDatabase();
        private readonly IAesEncryptionHelperService _aesEncryptionHelperService = aesEncryptionHelperService;

        public async Task<Response<string>> SetResetPassword(string email, string resetCode)
        {
            if (email is null)
                return Response<string>.Failure("email is null.");

            var cacheKey = $"resetpassword:value:{email}";
            var rateKey = $"resetpassword:rate:{email}";

            var lastSent = await _redis.StringGetAsync(rateKey);
            if (!string.IsNullOrEmpty(lastSent))
                return Response<string>.Failure("Please wait before requesting a new OTP.");

            // Set rate limit key for 58 seconds
            await _redis.StringSetAsync(rateKey, "1", TimeSpan.FromSeconds(58));

            var encrypted = _aesEncryptionHelperService.EncryptString(resetCode);

            await _redis.StringSetAsync(cacheKey, encrypted, TimeSpan.FromMinutes(15));

            return Response<string>.Success("Reset password code sent to your email.");
        }

        public async Task<Response<string>> GetResetPassword(string email)
        {
            var cacheKey = $"resetpassword:value:{email}";

            var value = await _redis.StringGetAsync(cacheKey);

            if (string.IsNullOrEmpty(value))
                return Response<string>.Failure("Reset password code not found or expired.");

            var decrypted = _aesEncryptionHelperService.DecryptString(value!);

            return Response<string>.Success(decrypted, "Reset password otp Successfly.");
        }

        public async Task<Response<string>> DeleteResetPassword(string email)
        {

            var cacheKey = $"resetpassword:value:{email}";
            await _redis.KeyDeleteAsync(cacheKey);
            return Response<string>.Success();
        }
    }
}
