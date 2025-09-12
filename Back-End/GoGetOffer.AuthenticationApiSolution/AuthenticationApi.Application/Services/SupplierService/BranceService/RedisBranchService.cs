using AuthenticationApi.Application.Services.Interfaces.SupplierService.BranceService;
using GoGetOffer.SharedLibrarySolution.DTOs.Supplier.Branch;
using GoGetOffer.SharedLibrarySolution.Interface.Helper;
using GoGetOffer.SharedLibrarySolution.Responses;
using MessagePack;
using StackExchange.Redis;

namespace AuthenticationApi.Application.Services.SupplierService.BranceService
{
    public class RedisBranchService(IConnectionMultiplexer redis,
        IAesEncryptionHelperService aesEncryptionHelperService) : IRedisBranchService
    {
        private readonly IDatabase _redis = redis.GetDatabase();
        private readonly IAesEncryptionHelperService _aesEncryptionHelperService = aesEncryptionHelperService;

        public async Task<Response<string>> DelAllBranchById(Guid id)
        {
            var key = $"Branch:all{id}";
            var DelSupplierer = await _redis.StringGetDeleteAsync(key);
            return Response<string>.Success("Branch Del from cache");
        }

        public async Task<Response<string>> DelBranchById(Guid id)
        {
            var key = $"Branch:{id}";
            var DelSupplierer = await _redis.StringGetDeleteAsync(key);
            return Response<string>.Success("Branch Del from cache");
        }

        public async Task<Response<byte[]>> GetAllBranchById(Guid id)
        {
            var key = $"Branch:all{id}";
            var encrypted = await _redis.StringGetAsync(key);

            if (!encrypted.HasValue)
                return Response<byte[]>.Failure("No value found");

            var decrypted = _aesEncryptionHelperService.Decrypt(encrypted!);


            await _redis.KeyExpireAsync(key, TimeSpan.FromHours(2));

            return Response<byte[]>.Success(decrypted, "Branch loaded from cache.");
        }

        public async Task<Response<byte[]>> GetBranchById(Guid id)
        {
            var key = $"Branch:{id}";
            var encrypted = await _redis.StringGetAsync(key);

            if (!encrypted.HasValue)
                return Response<byte[]>.Failure("No value found");

            var decrypted = _aesEncryptionHelperService.Decrypt(encrypted!);


            await _redis.KeyExpireAsync(key, TimeSpan.FromHours(2));

            return Response<byte[]>.Success(decrypted, "Branch loaded from cache.");
        }

        public async Task<Response<string>> SetAllBranchById(Guid id, IEnumerable<SupplierBranchDTO> dto)
        {
            if (dto is null)
                return Response<string>.Failure("dto is null.");

            var key = $"Branch:all{id}";

            var serialized = MessagePackSerializer.Serialize(dto);

            var encrypted = _aesEncryptionHelperService.Encrypt(serialized);

            await _redis.StringSetAsync(key, encrypted, TimeSpan.FromHours(2));

            return Response<string>.Success("Branch cached successfully.");
        }

        public async Task<Response<string>> SetBranchById(Guid id, SupplierBranchDTO dto)
        {
            if (dto is null)
                return Response<string>.Failure("dto is null.");

            var key = $"Branch:{id}";

            var serialized = MessagePackSerializer.Serialize(dto);

            var encrypted = _aesEncryptionHelperService.Encrypt(serialized);

            await _redis.StringSetAsync(key, encrypted, TimeSpan.FromHours(2));

            return Response<string>.Success("Branch cached successfully.");
        }
    }
}
