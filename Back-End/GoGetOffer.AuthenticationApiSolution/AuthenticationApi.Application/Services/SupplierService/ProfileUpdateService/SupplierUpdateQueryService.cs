using AuthenticationApi.Application.Services.Interfaces.SupplierService;
using AuthenticationApi.Application.Services.Interfaces.SupplierService.ProfileUpdateService;
using AuthenticationApi.Infrastructure.Repositories.Interfaces.Supplier;
using AutoMapper;
using GoGetOffer.SharedLibrarySolution.DTOs.Supplier.UpdateRequest;
using GoGetOffer.SharedLibrarySolution.Interface.Helper;
using GoGetOffer.SharedLibrarySolution.Responses;
using MessagePack;

namespace AuthenticationApi.Application.Services.SupplierService.ProfileUpdateService
{
    public class SupplierUpdateQueryService : ISupplierUpdateQueryService
    {
        private readonly IProfileSupplierUpdateRepository _profileSupplierUpdateRepository;
        private readonly IRedisSupplierUpdateService _redisSupplierUpdateService;
        private readonly IHelperMethodService _helperMethodService;
        private readonly ISupplierMethodsService _supplierMethodsService;
        private readonly IMapper _mapper;
        public SupplierUpdateQueryService(
            IProfileSupplierUpdateRepository profileSupplierUpdateRepository,
            IMapper mapper, IRedisSupplierUpdateService redisSupplierUpdateService,
            IHelperMethodService helperMethodService, ISupplierMethodsService supplierMethodsService)
        {
            _profileSupplierUpdateRepository = profileSupplierUpdateRepository;
            _redisSupplierUpdateService = redisSupplierUpdateService;
            _mapper = mapper;
            _helperMethodService = helperMethodService;
            _supplierMethodsService = supplierMethodsService;
        }

        public async Task<Response<IEnumerable<SupplierUpdateProfileDTO>>> GetAllRequestPending()
        {
            var Cache = await _redisSupplierUpdateService.GetAllRequestUpdate();
            if (Cache.Status && Cache.Data != null && Cache.Data.Length > 0)
            {
                var SuppObj = MessagePackSerializer.Deserialize<IEnumerable<SupplierUpdateProfileDTO>>(Cache.Data);
                return Response<IEnumerable<SupplierUpdateProfileDTO>>.Success(SuppObj, "Supplier Request retrieved from cache");
            }

            var CheckAndCache = await _helperMethodService.CheckAndCache("AllRequestPending", "all", 3, TimeSpan.FromMinutes(15));

            var response = await _profileSupplierUpdateRepository.GetAllAsync();
            if (!response.Status || response.Data is null)
                return Response<IEnumerable<SupplierUpdateProfileDTO>>.Failure(response.Message ?? "Failed to get users");

            var pendingRequests = response.Data.Where(x => x.IsApproved == Domain.Entites.Supplier.IsApproved.Pending).ToList();

            var decrept = pendingRequests
                .Select(_supplierMethodsService.DecryptProfileUpdate)
                .Where(r => r.Status && r.Data != null)
                .Select(r => r.Data!)
                .ToList();

            var SuppDtos = _mapper.Map<IEnumerable<SupplierUpdateProfileDTO>>(decrept);

            if (!CheckAndCache.Status)
                await _redisSupplierUpdateService.SetAllRequestUpdate(SuppDtos);

            return Response<IEnumerable<SupplierUpdateProfileDTO>>.Success(SuppDtos, "Supplieres Request From DB refreshed successfully.");
        }

        public async Task<Response<SupplierUpdateProfileDTO>> GetRequestById(Guid id)
        {
            var Cache = await
                _redisSupplierUpdateService.GetRequestById(id);

            if (Cache.Status && Cache.Data != null && Cache.Data.Length > 0)
            {
                var SuppObj = MessagePackSerializer.Deserialize<SupplierUpdateProfileDTO>(Cache.Data);
                return Response<SupplierUpdateProfileDTO>.Success(SuppObj, "Supplier Request retrieved from cache successfully.");
            }

            var request = await
                _profileSupplierUpdateRepository.FindByIdAsync(id);

            if (request.Data is null || !request.Status)
                return Response<SupplierUpdateProfileDTO>.Failure(request!.Message ?? " Request not found.");

            var CheckAndCache = await
                _helperMethodService.CheckAndCache("GetRequestById_SupplierUpdateProfileDTO", $"{id}", 3, TimeSpan.FromMinutes(15));

            var decrept = _supplierMethodsService.DecryptProfileUpdate(request.Data);

            if (decrept.Data is null)
                return Response<SupplierUpdateProfileDTO>.Failure("Decrypted data is null.");

            var dtorequest = _mapper.Map<SupplierUpdateProfileDTO>(decrept.Data);

            if (!CheckAndCache.Status)
                await _redisSupplierUpdateService.SetRequestById(dtorequest);

            return Response<SupplierUpdateProfileDTO>.Success(dtorequest, "Supplieres Request retrieved from DB successfully.");
        }
    }
}
