using AuthenticationApi.Application.Services.Interfaces.SupplierService.JoinService;
using AuthenticationApi.Infrastructure.Repositories.Interfaces.Supplier;
using AutoMapper;
using GoGetOffer.SharedLibrarySolution.DTOs.Supplier.JoinRequest;
using GoGetOffer.SharedLibrarySolution.Interface.Helper;
using GoGetOffer.SharedLibrarySolution.Responses;
using MessagePack;

namespace AuthenticationApi.Application.Services.SupplierService.JoinService
{
    public class SupplierJoinQueryService : ISupplierJoinQueryService
    {
        private readonly ISupplierJoinRequestRepository _supplierJoin;
        private readonly IRedisJoinService _redisJoinService;
        private readonly IHelperMethodService _helperMethodService;
        private readonly IMapper _mapper;

        public SupplierJoinQueryService(
        ISupplierJoinRequestRepository supplierJoin,
        IRedisJoinService redisJoinService,
        IHelperMethodService helperMethodService,
        IMapper mapper)
        {
            _supplierJoin = supplierJoin;
            _redisJoinService = redisJoinService;
            _helperMethodService = helperMethodService;
            _mapper = mapper;
        }
        public async Task<Response<IEnumerable<ProfileJoinRequestDTO>>> GetAllRequestPending()
        {
            var suppliersCache = await _redisJoinService.GetAllRequestJoinInfo();
            if (suppliersCache.Status && suppliersCache.Data != null && suppliersCache.Data.Length > 0)
            {
                var SuppObj = MessagePackSerializer.Deserialize<IEnumerable<ProfileJoinRequestDTO>>(suppliersCache.Data);
                return Response<IEnumerable<ProfileJoinRequestDTO>>.Success(SuppObj, "Supplier Request retrieved from cache");
            }

            var response = await RefreshGetAllRequestPending();
            return response.Status
                ? Response<IEnumerable<ProfileJoinRequestDTO>>.Success(response.Data, "Supplieres Request retrieved from database")
                : response;
        }

        public async Task<Response<IEnumerable<ProfileJoinRequestDTO>>> RefreshGetAllRequestPending()
        {

            var CheckAndCache = await _helperMethodService.CheckAndCache("RefreshProfiles", "all", 3, TimeSpan.FromMinutes(15));

            var response = await _supplierJoin.GetAllAsync();
            if (!response.Status || response.Data is null)
            {
                return Response<IEnumerable<ProfileJoinRequestDTO>>.Failure(response.Message ?? "Failed to get users");
            }
            _ = response.Data.Select(x => x.IsApproved == 0);

            var SuppDtos = _mapper.Map<IEnumerable<ProfileJoinRequestDTO>>(response.Data);

            if (!CheckAndCache.Status)
                await _redisJoinService.SetAllRequestJoinInfo(SuppDtos);

            return Response<IEnumerable<ProfileJoinRequestDTO>>.Success(SuppDtos, "Supplieres Request From DB refreshed successfully.");
        }

        public async Task<Response<ProfileJoinRequestDTO>> GetRequestById(Guid id)
        {
            var suppliersCache = await _redisJoinService.GetRequestByIdJoinInfo(id);
            if (suppliersCache.Status && suppliersCache.Data != null && suppliersCache.Data.Length > 0)
            {
                var SuppObj = MessagePackSerializer.Deserialize<ProfileJoinRequestDTO>(suppliersCache.Data);
                return Response<ProfileJoinRequestDTO>.Success(SuppObj, "Supplier Request retrieved from cache successfully.");
            }

            var response = await _supplierJoin.FindByIdAsync(id);
            if (!response.Status || response.Data is null)
            {
                return Response<ProfileJoinRequestDTO>.Failure(response.Message ?? "Failed to get users");
            }
            var SuppDtos = _mapper.Map<ProfileJoinRequestDTO>(response.Data);

            var CheckAndCache = await _helperMethodService.CheckAndCache("GetRequestById", $"{id}", 3, TimeSpan.FromMinutes(15));

            if (!CheckAndCache.Status)
                await _redisJoinService.SetRequestByIdJoinInfo(SuppDtos);

            return Response<ProfileJoinRequestDTO>.Success(SuppDtos, "Supplieres Request retrieved from DB successfully.");
        }
    }
}
