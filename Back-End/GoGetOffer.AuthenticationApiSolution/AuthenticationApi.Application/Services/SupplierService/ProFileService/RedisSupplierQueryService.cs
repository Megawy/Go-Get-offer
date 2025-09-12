using AuthenticationApi.Application.Services.Interfaces.SupplierService.ProFileService;
using GoGetOffer.SharedLibrarySolution.DTOs.Supplier.Profile;
using GoGetOffer.SharedLibrarySolution.Interface.Helper;
using GoGetOffer.SharedLibrarySolution.Responses;
using MessagePack;
using StackExchange.Redis;

namespace AuthenticationApi.Application.Services.SupplierService.ProFileService
{
    public class RedisSupplierQueryService
        (IConnectionMultiplexer redis,
        IAesEncryptionHelperService aesEncryptionHelperService) : IRedisSupplierQueryService
    {
        private readonly IDatabase _redis = redis.GetDatabase();
        private readonly IAesEncryptionHelperService _aesEncryptionHelperService = aesEncryptionHelperService;

        public async Task<Response<byte[]>> GetAllProfileInfo()
        {
            var key = $"Supplieres:all";
            var encrypted = await _redis.StringGetAsync(key);

            if (!encrypted.HasValue)
                return Response<byte[]>.Failure("No value found");

            var decrypted = _aesEncryptionHelperService.Decrypt(encrypted!);


            await _redis.KeyExpireAsync(key, TimeSpan.FromHours(8));

            return Response<byte[]>.Success(decrypted, "Supplier loaded from cache.");
        }

        public async Task<Response<string>> DelAllProfileInfo()
        {
            var key = $"Supplieres:all";
            var DelSupplierer = await _redis.StringGetDeleteAsync(key);
            return Response<string>.Success("Supplier Del from cache");
        }

        public async Task<Response<string>> SetAllProfileInfo(IEnumerable<SupplierProfileDTO> dto)
        {
            if (dto is null)
                return Response<string>.Failure("dto is null.");

            var key = $"Supplieres:all";

            var serialized = MessagePackSerializer.Serialize(dto);

            var encrypted = _aesEncryptionHelperService.Encrypt(serialized);

            await _redis.StringSetAsync(key, encrypted, TimeSpan.FromHours(8));

            return Response<string>.Success("Supplers cached successfully.");
        }



        public async Task<Response<byte[]>> GetProfileInfoById(Guid Id)
        {
            var key = $"Supplier_Id:{Id}";
            var encrypted = await _redis.StringGetAsync(key);

            if (!encrypted.HasValue)
                return Response<byte[]>.Failure("No value found");

            var decrypted = _aesEncryptionHelperService.Decrypt(encrypted!);


            await _redis.KeyExpireAsync(key, TimeSpan.FromHours(1));

            return Response<byte[]>.Success(decrypted, "Supplier loaded from cache.");
        }

        public async Task<Response<string>> SetProfileInfoById(SupplierProfileDTO dto)
        {
            if (dto is null)
                return Response<string>.Failure("dto is null.");

            var key = $"Supplier_Id:{dto.Id}";

            var serialized = MessagePackSerializer.Serialize(dto);

            var encrypted = _aesEncryptionHelperService.Encrypt(serialized);

            await _redis.StringSetAsync(key, encrypted, TimeSpan.FromHours(1));

            return Response<string>.Success("User cached successfully.");
        }

        public async Task<Response<string>> DelProfileInfoById(Guid Id)
        {
            var key = $"Supplier_Id:{Id}";
            var DelSupplier = await _redis.StringGetDeleteAsync(key);
            return Response<string>.Success("Supplier Del from cache");
        }



        public async Task<Response<byte[]>> GetProfileInfoByCode(CodeSupplierDTO dto)
        {
            var key = $"Supplier_Id:{dto.code!.ToUpper()}";
            var encrypted = await _redis.StringGetAsync(key);

            if (!encrypted.HasValue)
                return Response<byte[]>.Failure("No value found");

            var decrypted = _aesEncryptionHelperService.Decrypt(encrypted!);


            await _redis.KeyExpireAsync(key, TimeSpan.FromHours(1));

            return Response<byte[]>.Success(decrypted, "Supplier loaded from cache.");
        }

        public async Task<Response<string>> SetProfileInfoByCode(SupplierProfileDTO dto)
        {
            if (dto is null)
                return Response<string>.Failure("dto is null.");

            var key = $"Supplier_Id:{dto.Code!.ToUpper()}";

            var serialized = MessagePackSerializer.Serialize(dto);

            var encrypted = _aesEncryptionHelperService.Encrypt(serialized);

            await _redis.StringSetAsync(key, encrypted, TimeSpan.FromHours(1));

            return Response<string>.Success("Profile cached successfully.");
        }

        public async Task<Response<string>> DelProfileInfoByCode(CodeSupplierDTO dto)
        {
            var key = $"Supplier_Id:{dto.code!.ToUpper()}";
            var DelSupplier = await _redis.StringGetDeleteAsync(key);
            return Response<string>.Success("Supplier Del from cache");
        }
    }
}
