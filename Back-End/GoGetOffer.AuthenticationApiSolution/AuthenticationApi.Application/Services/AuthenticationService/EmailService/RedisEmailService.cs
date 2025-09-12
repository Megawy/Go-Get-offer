using AuthenticationApi.Application.Services.Interfaces.AuthenticationService.EmailService;
using GoGetOffer.SharedLibrarySolution.Interface.Helper;
using GoGetOffer.SharedLibrarySolution.Responses;
using StackExchange.Redis;

namespace AuthenticationApi.Application.Services.AuthenticationService.EmailService
{
    public class RedisEmailService
       (IConnectionMultiplexer redis,
        IAesEncryptionHelperService aesEncryptionHelperService) : IRedisEmailService
    {
        private readonly IDatabase _redis = redis.GetDatabase();
        private readonly IAesEncryptionHelperService _aesEncryptionHelperService = aesEncryptionHelperService;

        public async Task<Response<string>> GetOTP(Guid id)
        {
            var valueKey = $"otp:id:value:{id}";
            var cachedOtp = await _redis.StringGetAsync(valueKey);

            if (string.IsNullOrEmpty(cachedOtp))
                return Response<string>.Failure("OTP not found or expired.");

            var decrypted = _aesEncryptionHelperService.DecryptString(cachedOtp!);

            return Response<string>.Success(decrypted, "OTP loaded from cache");
        }

        public async Task<Response<string>> SetOTP(Guid id, string otp)
        {
            if (otp is null)
                return Response<string>.Failure("otp is null.");

            var valueKey = $"otp:id:value:{id}";
            var rateKey = $"otp:id:rate:{id}";
            var encrypted = _aesEncryptionHelperService.EncryptString(otp);

            // Check if OTP was recently sent (rate limit)
            var lastSent = await _redis.StringGetAsync(rateKey);
            if (!string.IsNullOrEmpty(lastSent))
                return Response<string>.Failure("Please wait before requesting a new OTP.");

            // Set rate limit key for 58 seconds
            await _redis.StringSetAsync(rateKey, "1", TimeSpan.FromSeconds(58));


            // Set OTP value for 15 minute
            await _redis.StringSetAsync(valueKey, encrypted, TimeSpan.FromMinutes(15));

            return Response<string>.Success("OTP cached successfully.");
        }

        public async Task<Response<string>> DelOTP(Guid id)
        {
            var valueKey = $"otp:id:value:{id}";
            var DelUsers = await _redis.StringGetDeleteAsync(valueKey);
            return Response<string>.Success("OTP Del from cache");
        }
    }
}
