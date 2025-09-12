using AuthenticationApi.Application.Services.Interfaces.SupplierService;
using AuthenticationApi.Application.Services.Interfaces.SupplierService.ProFileService;
using AuthenticationApi.Infrastructure.Repositories.Interfaces.Supplier;
using AutoMapper;
using GoGetOffer.SharedLibrarySolution.DTOs.Supplier.Profile;
using GoGetOffer.SharedLibrarySolution.Interface.Helper;
using GoGetOffer.SharedLibrarySolution.Responses;
using MessagePack;

namespace AuthenticationApi.Application.Services.SupplierService.ProFileService
{
    public class SupplierQueryService
        (ISupplierRepository supplierRepository,
        ISupplierMethodsService supplierMethods,
        IRedisSupplierQueryService redisSupplierQuery,
        IHelperMethodService helperMethodService,
        IMapper mapper) : ISupplierQueryService
    {
        private readonly ISupplierRepository _supplierRepository = supplierRepository;
        private readonly ISupplierMethodsService _supplierMethods = supplierMethods;
        private readonly IRedisSupplierQueryService _redisSupplierQuery = redisSupplierQuery;
        private readonly IHelperMethodService _helperMethodService = helperMethodService;
        private readonly IMapper _mapper = mapper;

        public async Task<Response<SupplierProfileDTO>> GetProfileById(Guid id)
        {
            var SuppCache = await _redisSupplierQuery.GetProfileInfoById(id);
            if (SuppCache.Status && SuppCache.Data is not null)
            {
                var suppObj = MessagePackSerializer.Deserialize<SupplierProfileDTO>(SuppCache.Data);
                return Response<SupplierProfileDTO>.Success(suppObj, "Supplier retrieved from cache");
            }
            var supplier = await RefreshProfileById(id);
            return supplier.Status
                ? Response<SupplierProfileDTO>.Success(supplier.Data, "supplier retrieved from database")
                : supplier;
        }

        public async Task<Response<SupplierProfileDTO>> RefreshProfileById(Guid id)
        {
            var CheckAndCache = await _helperMethodService.CheckAndCache("ProfileById", $"{id}", 3, TimeSpan.FromMinutes(15));

            var getProfile = await _supplierRepository.FindByIdAsync(id);
            if (!getProfile.Status)
                return Response<SupplierProfileDTO>.Failure(getProfile.Message!);

            var decrept_profile = _supplierMethods.DecryptProfileData(getProfile.Data!);

            var result_profile = _mapper.Map<SupplierProfileDTO>(decrept_profile.Data);

            if (!CheckAndCache.Status)
                await _redisSupplierQuery.SetProfileInfoById(result_profile);

            return Response<SupplierProfileDTO>.Success(result_profile, "successfully get profile.");
        }


        public async Task<Response<IEnumerable<SupplierProfileDTO>>> GetProfiles()
        {
            var suppliersCache = await _redisSupplierQuery.GetAllProfileInfo();
            if (suppliersCache.Status && suppliersCache.Data != null && suppliersCache.Data.Length > 0)
            {
                var SuppObj = MessagePackSerializer.Deserialize<IEnumerable<SupplierProfileDTO>>(suppliersCache.Data);
                return Response<IEnumerable<SupplierProfileDTO>>.Success(SuppObj, "Supplier retrieved from cache");
            }

            var response = await RefreshProfiles();
            return response.Status
                ? Response<IEnumerable<SupplierProfileDTO>>.Success(response.Data, "Supplieres retrieved from database")
                : response;
        }

        public async Task<Response<IEnumerable<SupplierProfileDTO>>> RefreshProfiles()
        {

            var CheckAndCache = await _helperMethodService.CheckAndCache("RefreshProfiles", "all", 3, TimeSpan.FromMinutes(15));

            var response = await _supplierRepository.GetAllAsync(true);
            if (!response.Status || response.Data is null)
            {
                return Response<IEnumerable<SupplierProfileDTO>>.Failure(response.Message ?? "Failed to get users");
            }

            var decryptedSupplieres = response.Data
                .Select(_supplierMethods.DecryptProfileData)
                    .Where(r => r.Status && r.Data != null)
                        .Select(r => r.Data!)
                            .ToList();

            var SuppDtos = _mapper.Map<IEnumerable<SupplierProfileDTO>>(decryptedSupplieres);

            if (!CheckAndCache.Status)
                await _redisSupplierQuery.SetAllProfileInfo(SuppDtos);

            return Response<IEnumerable<SupplierProfileDTO>>.Success(SuppDtos, "Users cache refreshed successfully.");
        }


        public async Task<Response<SupplierProfileDTO>> GetProfileByCode(CodeSupplierDTO dto)
        {
            var SuppCache = await _redisSupplierQuery.GetProfileInfoByCode(dto);
            if (SuppCache.Status && SuppCache.Data is not null)
            {
                var suppObj = MessagePackSerializer.Deserialize<SupplierProfileDTO>(SuppCache.Data);
                return Response<SupplierProfileDTO>.Success(suppObj, "Supplier retrieved from cache");
            }
            var supplier = await RefreshProfileByCode(dto);
            return supplier.Status
                ? Response<SupplierProfileDTO>.Success(supplier.Data, "supplier retrieved from database")
                : Response<SupplierProfileDTO>.Failure("Failed retrieved from database");
        }

        public async Task<Response<SupplierProfileDTO>> RefreshProfileByCode(CodeSupplierDTO dto)
        {
            var CheckAndCache = await _helperMethodService.CheckAndCache("RefreshProfileByCode", $"{dto.code}", 3, TimeSpan.FromMinutes(15));

            var getProfile = await _supplierRepository.GetByAsync(x => x.Code == dto.code!.ToUpper());
            if (getProfile.Data is null)
                return Response<SupplierProfileDTO>.Failure("Not Founde Profile.");

            var decrept_profile = _supplierMethods.DecryptProfileData(getProfile.Data!);

            var result_profile = _mapper.Map<SupplierProfileDTO>(decrept_profile.Data);

            if (!CheckAndCache.Status)
                await _redisSupplierQuery.SetProfileInfoByCode(result_profile);

            return Response<SupplierProfileDTO>.Success(result_profile, "successfully get profile.");
        }
    }
}

