using AuthenticationApi.Application.Services.Interfaces.SupplierService.ProfileUpdateService;
using GoGetOffer.SharedLibrarySolution.DTOs.Supplier.UpdateRequest;
using GoGetOffer.SharedLibrarySolution.Interface.Helper;
using GoGetOffer.SharedLibrarySolution.Responses;
using MessagePack;
using StackExchange.Redis;

namespace AuthenticationApi.Application.Services.SupplierService.ProfileUpdateService
{
    public class RedisSupplierUpdateService : IRedisSupplierUpdateService
    {
        private readonly IDatabase _redis;
        private readonly IAesEncryptionHelperService _aesEncryptionHelperService;
        public RedisSupplierUpdateService(IConnectionMultiplexer redis,
        IAesEncryptionHelperService aesEncryptionHelperService)
        {
            _redis = redis.GetDatabase();
            _aesEncryptionHelperService = aesEncryptionHelperService;
        }

        public async Task<Response<byte[]>> GetAllRequestUpdate()
        {
            var key = $"Supplieres_Request:all";
            var encrypted = await _redis.StringGetAsync(key);

            if (!encrypted.HasValue)
                return Response<byte[]>.Failure("No value found");

            var decrypted = _aesEncryptionHelperService.Decrypt(encrypted!);

            await _redis.KeyExpireAsync(key, TimeSpan.FromHours(8));

            return Response<byte[]>.Success(decrypted, "Supplier Request loaded from cache.");
        }

        public async Task<Response<string>> SetAllRequestUpdate(IEnumerable<SupplierUpdateProfileDTO> dto)
        {
            if (dto is null)
                return Response<string>.Failure("Supplers Request Can't cached successfully.");

            var key = $"Supplieres_Request:all";

            var serialized = MessagePackSerializer.Serialize(dto);

            var encrypted = _aesEncryptionHelperService.Encrypt(serialized);

            await _redis.StringSetAsync(key, encrypted, TimeSpan.FromHours(8));

            return Response<string>.Success("Supplers Request cached successfully.");
        }

        public async Task<Response<string>> DelAllRequestUpdate()
        {
            var key = $"Supplieres_Request:all";
            var DelSupplierer = await _redis.StringGetDeleteAsync(key);
            return Response<string>.Success("Supplier Del from cache");
        }



        public async Task<Response<byte[]>> GetRequestById(Guid id)
        {
            var key = $"Supplieres_Request_Edit:{id}";
            var encrypted = await _redis.StringGetAsync(key);

            if (!encrypted.HasValue)
                return Response<byte[]>.Failure("No value found");

            var decrypted = _aesEncryptionHelperService.Decrypt(encrypted!);


            await _redis.KeyExpireAsync(key, TimeSpan.FromMinutes(15));

            return Response<byte[]>.Success(decrypted, "Supplier Request loaded from cache.");
        }

        public async Task<Response<string>> SetRequestById(SupplierUpdateProfileDTO dto)
        {
            if (dto is null)
                return Response<string>.Failure("Supplers Request Can't cached successfully.");

            var key = $"Supplieres_Request_Edit:{dto.SupplierProfilesId}";

            var serialized = MessagePackSerializer.Serialize(dto);

            var encrypted = _aesEncryptionHelperService.Encrypt(serialized);

            await _redis.StringSetAsync(key, encrypted, TimeSpan.FromMinutes(15));

            return Response<string>.Success("Supplers Request cached successfully.");
        }

        public async Task<Response<string>> DelRequestById(Guid id)
        {
            var key = $"Supplieres_Request_Edit:{id}";
            var DelSupplierer = await _redis.StringGetDeleteAsync(key);
            return Response<string>.Success("Supplier Request Del from cache");
        }
    }
}
