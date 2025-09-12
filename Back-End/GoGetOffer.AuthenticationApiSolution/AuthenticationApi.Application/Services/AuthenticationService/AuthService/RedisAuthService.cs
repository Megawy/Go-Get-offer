using AuthenticationApi.Application.Services.Interfaces.AuthenticationService.AuthService;
using GoGetOffer.SharedLibrarySolution.DTOs.Auth.User.Login;
using GoGetOffer.SharedLibrarySolution.DTOs.Auth.User.UserProfile;
using GoGetOffer.SharedLibrarySolution.Interface.Helper;
using GoGetOffer.SharedLibrarySolution.Interface.JWT;
using GoGetOffer.SharedLibrarySolution.Responses;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;

namespace AuthenticationApi.Application.Services.AuthenticationService.AuthService
{
    public class RedisAuthService
        (IConnectionMultiplexer redis,
        IJwtService jwtService,
        IAesEncryptionHelperService aesEncryptionHelperService) : IRedisAuthService
    {
        private readonly IDatabase _redis = redis.GetDatabase();
        private readonly IJwtService _jwtService = jwtService;
        private readonly IAesEncryptionHelperService _aesEncryptionHelperService = aesEncryptionHelperService;

        public async Task<Response<string>> GetTokenLogin(Guid id)
        {
            var key = $"token:user:{id}";
            var token = await _redis.StringGetAsync(key);

            if (string.IsNullOrEmpty(token))
                return Response<string>.Failure("Token not found in cache.");

            var decryptString = _aesEncryptionHelperService.DecryptString(token!);

            return Response<string>.Success(decryptString, "Token TTL renewed.");
        }

        public async Task<Response<string>> SetTokenLogin(UserDTO userDTO, string token, TimeSpan timeSpan)
        {
            if (token is null)
                return Response<string>.Failure("token is null.");

            var key = $"token:user:{userDTO.Id}";

            var encrypt = _aesEncryptionHelperService.EncryptString(token);

            await _redis.StringSetAsync(key, encrypt, timeSpan);

            return Response<string>.Success("Token cached successfully.");
        }

        public async Task<Response<TokenDTO>> RefreshTokenLogin(UserDTO userDto, string refreshToken)
        {
            if (refreshToken is null)
                return Response<TokenDTO>.Failure("refreshToken is null.");

            var key = $"refresh:user:{refreshToken}";
            var cachedToken = await _redis.StringGetAsync(key);

            var decryptString = _aesEncryptionHelperService.DecryptString(cachedToken!);

            if (cachedToken.IsNullOrEmpty || decryptString != refreshToken || refreshToken.IsNullOrEmpty())
                return Response<TokenDTO>.Failure("Invalid or expired refresh token.");

            var newAccessToken = _jwtService.GenerateRefreshToken(userDto);

            if (newAccessToken is null)
                return Response<TokenDTO>.Failure("Invalid refresh token.");

            await _redis.KeyDeleteAsync($"refresh:user:{refreshToken}");
            await _redis.KeyDeleteAsync($"token:user:{refreshToken}");
            await SetRefreshToken(refreshToken);

            return Response<TokenDTO>.Success(new TokenDTO
            {
                AccessToken = newAccessToken,
            }, "Token refreshed successfully.");
        }

        public async Task<Response<string>> SetRefreshToken(string refreshToken)
        {
            if (refreshToken is null)
                return Response<string>.Failure("refreshToken is null.");

            var key = $"refresh:user:{refreshToken}";
            var encrypted = _aesEncryptionHelperService.EncryptString(refreshToken);
            await _redis.StringSetAsync(key, encrypted, TimeSpan.FromDays(8));
            return Response<string>.Success("Refresh token stored successfully.");
        }

        public async Task<Response<string>> DelTokenLogin(Guid id)
        {
            await _redis.KeyDeleteAsync($"refresh:user:{id}");
            await _redis.KeyDeleteAsync($"token:user:{id}");
            return Response<string>.Success("Token Del from cache");
        }
    }
}
