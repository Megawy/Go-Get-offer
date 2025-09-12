using AuthenticationApi.Application.Services.Interfaces.AuthenticationService.QueryService;
using GoGetOffer.SharedLibrarySolution.DTOs.Auth.User.UserProfile;
using GoGetOffer.SharedLibrarySolution.Interface.Helper;
using GoGetOffer.SharedLibrarySolution.Responses;
using MessagePack;
using StackExchange.Redis;

namespace AuthenticationApi.Application.Services.AuthenticationService.QueryService
{
    public class RedisQueryService
        (IConnectionMultiplexer redis,
        IAesEncryptionHelperService aesEncryptionHelperService) : IRedisQueryService
    {
        private readonly IDatabase _redis = redis.GetDatabase();
        private readonly IAesEncryptionHelperService _aesEncryptionHelperService = aesEncryptionHelperService;

        public async Task<Response<byte[]>> GetUserInfo(Guid Id)
        {
            var key = $"User:{Id}";
            var encrypted = await _redis.StringGetAsync(key);

            if (!encrypted.HasValue)
                return Response<byte[]>.Failure("No value found");

            var decrypted = _aesEncryptionHelperService.Decrypt((byte[])encrypted!);


            await _redis.KeyExpireAsync(key, TimeSpan.FromHours(1));

            return Response<byte[]>.Success(decrypted, "User loaded from cache.");
        }

        public async Task<Response<string>> SetUserInfo(UserDTO userDTO)
        {
            if (userDTO is null)
                return Response<string>.Failure("userDTO is null.");

            var key = $"User:{userDTO.Id}";
            var serialized = MessagePackSerializer.Serialize(userDTO);
            var encrypted = _aesEncryptionHelperService.Encrypt(serialized);
            await _redis.StringSetAsync(key, encrypted, TimeSpan.FromHours(1));
            return Response<string>.Success("User cached successfully.");
        }

        public async Task<Response<string>> DelUserInfo(Guid id)
        {
            var key = $"User:{id}";
            var DelUser = await _redis.StringGetDeleteAsync(key);
            return Response<string>.Success("User Del from cache");
        }

        public async Task<Response<byte[]>> GetAllUser()
        {
            var key = "Users:all";
            var encrypted = await _redis.StringGetAsync(key);

            if (!encrypted.HasValue)
                return Response<byte[]>.Failure("No value found");

            var decrypted = _aesEncryptionHelperService.Decrypt((byte[])encrypted!);

            await _redis.KeyExpireAsync(key, TimeSpan.FromHours(1));

            return Response<byte[]>.Success(decrypted, "Users loaded from cache and TTL renewed.");
        }


        public async Task<Response<string>> SetAllUser(IEnumerable<UserDTO> users)
        {
            if (users is null)
                return Response<string>.Failure("users is null.");

            var key = "Users:all";
            var serialized = MessagePackSerializer.Serialize(users);
            var encrypted = _aesEncryptionHelperService.Encrypt(serialized);
            await _redis.StringSetAsync(key, encrypted, TimeSpan.FromHours(1));
            return Response<string>.Success("All users cached successfully.");

        }

        public async Task<Response<string>> DelUsers()
        {
            var key = "Users:all";
            var DelUsers = await _redis.StringGetDeleteAsync(key);
            return Response<string>.Success("Users Del from cache");
        }
    }
}

