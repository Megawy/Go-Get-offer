using AuthenticationApi.Application.Services.Interfaces.SupplierService;
using AuthenticationApi.Application.Services.Interfaces.SupplierService.BranceService;
using AuthenticationApi.Infrastructure.Repositories.Interfaces.Supplier;
using AutoMapper;
using GoGetOffer.SharedLibrarySolution.DTOs.Supplier.Branch;
using GoGetOffer.SharedLibrarySolution.DTOs.Supplier.Profile;
using GoGetOffer.SharedLibrarySolution.Interface.Helper;
using GoGetOffer.SharedLibrarySolution.Responses;
using MessagePack;

namespace AuthenticationApi.Application.Services.SupplierService.BranceService
{
    public class SupplierBranceQueryService
        (ISupplierBranchRepository supplierBranch,
        ISupplierMethodsService supplierMethods,
        IRedisBranchService redisBranchService,
        IHelperMethodService helperMethodService,
        IMapper mapper) : ISupplierBranceQueryService
    {
        private readonly ISupplierBranchRepository _supplierBranch = supplierBranch;
        private readonly ISupplierMethodsService _supplierMethods = supplierMethods;
        private readonly IRedisBranchService _redisBranchService = redisBranchService;
        private readonly IHelperMethodService _helperMethodService = helperMethodService;
        private readonly IMapper _mapper = mapper;

        public async Task<Response<IEnumerable<SupplierBranchDTO>>> GetAllBranchByUserId(Guid id)
        {
            var BranchCache = await _redisBranchService.GetAllBranchById(id);
            if (BranchCache.Status && BranchCache.Data is not null)
            {
                var BranchObj = MessagePackSerializer.Deserialize<IEnumerable<SupplierBranchDTO>>(BranchCache.Data);
                return Response<IEnumerable<SupplierBranchDTO>>.Success(BranchObj, "Branches retrieved from cache");
            }

            var CheckAndCache = await _helperMethodService.CheckAndCache("GetAllBranchByUserId", $"{id}", 2, TimeSpan.FromMinutes(15));

            var response = await _supplierBranch.GetAllBranchByUserId(id);

            if (!response.Status || response.Data is null)
                return Response<IEnumerable<SupplierBranchDTO>>.Failure(response.Message ?? "Failed to get Brances");

            var decryptedBrances = response.Data
                .Select(_supplierMethods.DecryptBranceData)
                    .Where(r => r.Status && r.Data != null)
                        .Select(r => r.Data!)
                            .ToList();

            var dtomap = _mapper.Map<IEnumerable<SupplierBranchDTO>>(decryptedBrances);

            if (!CheckAndCache.Status)
                await _redisBranchService.SetAllBranchById(id, dtomap);

            return Response<IEnumerable<SupplierBranchDTO>>.Success(dtomap, "Branches retrieved from database");
        }

        public async Task<Response<IEnumerable<SupplierBranchDTO>>> GetBranchByCode(CodeSupplierDTO dto)
        {
            var branches = await _supplierBranch.GetAllBranchByUserCode(dto);
            if (!branches.Status)
                return Response<IEnumerable<SupplierBranchDTO>>.Failure(branches.Message ?? "Branch retrieved from cache");

            var decryptedBrances = branches.Data!
               .Select(_supplierMethods.DecryptBranceData)
                   .Where(r => r.Status && r.Data != null)
                       .Select(r => r.Data!)
                           .ToList();

            var result_profile = _mapper.Map<IEnumerable<SupplierBranchDTO>>(decryptedBrances);

            return Response<IEnumerable<SupplierBranchDTO>>.Success(result_profile, "Branches retrieved from database");
        }

        public async Task<Response<SupplierBranchDTO>> GetBranchById(Guid id)
        {
            var BranchCache = await _redisBranchService.GetBranchById(id);
            if (BranchCache.Status && BranchCache.Data is not null)
            {
                var BranchObj = MessagePackSerializer.Deserialize<SupplierBranchDTO>(BranchCache.Data);
                return Response<SupplierBranchDTO>.Success(BranchObj, "Branch retrieved from cache");
            }

            var CheckAndCache = await _helperMethodService.CheckAndCache("GetBranchById", $"{id}", 2, TimeSpan.FromMinutes(15));

            var getProfile = await
                _supplierBranch.FindUserNoTracking(id);

            if (!getProfile.Status)
                return Response<SupplierBranchDTO>.Failure(getProfile.Message!);

            var decrept_profile = _supplierMethods.DecryptBranceData(getProfile.Data!);
            var result_profile = _mapper.Map<SupplierBranchDTO>(decrept_profile.Data);

            if (!CheckAndCache.Status)
                await _redisBranchService.SetBranchById(id, result_profile);

            return Response<SupplierBranchDTO>.Success(result_profile, "Branch retrieved from database.");
        }
    }
}
