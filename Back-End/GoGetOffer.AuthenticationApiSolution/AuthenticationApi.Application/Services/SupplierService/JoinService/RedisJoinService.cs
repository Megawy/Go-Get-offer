using AuthenticationApi.Application.Services.Interfaces.SupplierService.JoinService;
using GoGetOffer.SharedLibrarySolution.DTOs.Supplier.JoinRequest;
using GoGetOffer.SharedLibrarySolution.Interface.Helper;
using GoGetOffer.SharedLibrarySolution.Responses;
using MessagePack;
using StackExchange.Redis;

namespace AuthenticationApi.Application.Services.SupplierService.JoinService
{
    public class RedisJoinService(IConnectionMultiplexer redis,
        IAesEncryptionHelperService aesEncryptionHelperService) : IRedisJoinService
    {
        private readonly IDatabase _redis = redis.GetDatabase();
        private readonly IAesEncryptionHelperService _aesEncryptionHelperService = aesEncryptionHelperService;

        public async Task<Response<string>> DelAllRequestJoinInfo()
        {
                var key = $"Supplieres_Request:all";
                var DelSupplierer = await _redis.StringGetDeleteAsync(key);
                return Response<string>.Success("Supplier Request Del from cache");
        }

        public async Task<Response<string>> DelRequestByIdJoinInfo(Guid id)
        {
                var key = $"Supplieres_Request:{id}";
                var DelSupplierer = await _redis.StringGetDeleteAsync(key);
                return Response<string>.Success("Supplier Request Del from cache");
        }

        public async Task<Response<byte[]>> GetAllRequestJoinInfo()
        {
                var key = $"Supplieres_Request:all";
                var encrypted = await _redis.StringGetAsync(key);

                if (!encrypted.HasValue)
                    return Response<byte[]>.Failure("No value found");

                var decrypted = _aesEncryptionHelperService.Decrypt(encrypted!);


                await _redis.KeyExpireAsync(key, TimeSpan.FromHours(2));

                return Response<byte[]>.Success(decrypted, "Supplier Request loaded from cache.");
        }

        public async Task<Response<byte[]>> GetRequestByIdJoinInfo(Guid id)
        {
                var key = $"Supplieres_Request:{id}";
                var encrypted = await _redis.StringGetAsync(key);

                if (!encrypted.HasValue)
                    return Response<byte[]>.Failure("No value found");

                var decrypted = _aesEncryptionHelperService.Decrypt(encrypted!);


                await _redis.KeyExpireAsync(key, TimeSpan.FromMinutes(15));

                return Response<byte[]>.Success(decrypted, "Supplier Request loaded from cache.");
        }

        public async Task<Response<string>> SetAllRequestJoinInfo(IEnumerable<ProfileJoinRequestDTO> dto)
        {
            if (dto is null)
                return Response<string>.Failure("dto is null.");

            var key = $"Supplieres_Request:all";

                var serialized = MessagePackSerializer.Serialize(dto);

                var encrypted = _aesEncryptionHelperService.Encrypt(serialized);

                await _redis.StringSetAsync(key, encrypted, TimeSpan.FromHours(2));

                return Response<string>.Success("Supplers cached successfully.");
        }

        public async Task<Response<string>> SetRequestByIdJoinInfo(ProfileJoinRequestDTO dto)
        {
            if (dto is null)
                return Response<string>.Failure("dto is null.");

            var key = $"Supplieres_Request:{dto.Id}";

                var serialized = MessagePackSerializer.Serialize(dto);

                var encrypted = _aesEncryptionHelperService.Encrypt(serialized);

                await _redis.StringSetAsync(key, encrypted, TimeSpan.FromMinutes(15));

                return Response<string>.Success("Supplers Request cached successfully.");
        }
    }
}
