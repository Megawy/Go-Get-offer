using AuthenticationApi.Application.Services.Interfaces.SupplierService;
using AuthenticationApi.Application.Services.Interfaces.SupplierService.BranceService;
using AuthenticationApi.Domain.Entites.Supplier;
using AuthenticationApi.Infrastructure.Repositories.Interfaces.Supplier;
using AutoMapper;
using GoGetOffer.SharedLibrarySolution.DTOs.Supplier.Branch;
using GoGetOffer.SharedLibrarySolution.Responses;
using Hangfire;

namespace AuthenticationApi.Application.Services.SupplierService.BranceService
{
    public class SupplierBranchCommandService
        (ISupplierBranchRepository branchRepository,
        ISupplierMethodsService supplierMethods,
        IRedisBranchService redisBranchService,
        IMapper mapper) : ISupplierBranchCommandService
    {
        private readonly ISupplierBranchRepository _branchRepository = branchRepository;
        private readonly ISupplierMethodsService _supplierMethods = supplierMethods;
        private readonly IRedisBranchService _redisBranchService = redisBranchService;
        private readonly IMapper _mapper = mapper;

        public async Task<Response<SupplierBranchDTO>> CreateBranch(CreateBranchDTO dto)
        {
            var encrpt = _supplierMethods.CreateEncryptedBranch(dto);
            var BranchEntity = _mapper.Map<SupplierBranch>(encrpt.Data);
            var newBranch = await _branchRepository.CreateAsync(BranchEntity);

            if (!newBranch.Status || newBranch.Data is null)
                return Response<SupplierBranchDTO>.Failure(newBranch.Message ?? "Can't Create Branch.");

            var decrept = _supplierMethods.DecryptBranceData(newBranch.Data!);
            var result = _mapper.Map<SupplierBranchDTO>(decrept.Data);

            BackgroundJob.Enqueue(() => _redisBranchService.DelAllBranchById(dto.SupplierProfilesId));

            return Response<SupplierBranchDTO>.Success(result, "Create Branch successfully.");
        }

        public async Task<Response<SupplierBranchDTO>> DeleteBranch(Guid id)
        {
            var getBranch = await
                _branchRepository.FindByIdAsync(id);

            if (!getBranch.Status || getBranch.Data is null)
                return Response<SupplierBranchDTO>.Failure(getBranch.Message ?? "Branch not found");

            if (getBranch.Data.Main_Branch == true)
                return Response<SupplierBranchDTO>.Failure("Can't delete Main Branch.");

            var deleteBranch = await _branchRepository.DeleteAsync(getBranch.Data);

            if (!deleteBranch.Status)
                return Response<SupplierBranchDTO>.Failure(deleteBranch.Message ?? "Can't delete Branch");

            BackgroundJob.Enqueue(() => _redisBranchService.DelAllBranchById(getBranch.Data.SupplierProfilesId));
            BackgroundJob.Enqueue(() => _redisBranchService.DelBranchById(getBranch.Data.SupplierProfilesId));

            return Response<SupplierBranchDTO>.Success("Delete Branch successfully.");
        }

        public async Task<Response<SupplierBranchDTO>> UpdateBranch(UpdateBranchDTO dto)
        {
            var getBranch = await _branchRepository.FindByIdAsync(dto.Id!.Value);

            if (!getBranch.Status || getBranch.Data is null)
                return Response<SupplierBranchDTO>.Failure(getBranch.Message ?? "Branch not found");

            if (getBranch.Data.Main_Branch == true)
                return Response<SupplierBranchDTO>.Failure("Can't update Main Branch.");

            var encrpt = _supplierMethods.UpdateEncryptedBranch(dto);

            _mapper.Map(encrpt.Data, getBranch.Data);

            var UpdateBranch = await _branchRepository.UpdateAsync(getBranch.Data);

            if (!UpdateBranch.Status)
                return Response<SupplierBranchDTO>.Failure(UpdateBranch.Message ?? "Can't Update Branch");

            var decrept = _supplierMethods.DecryptBranceData(UpdateBranch.Data!);
            if (!decrept.Status)
                return Response<SupplierBranchDTO>.Failure(decrept.Message ?? "Can't decrept Branch");

            var result = _mapper.Map<SupplierBranchDTO>(decrept.Data);

            BackgroundJob.Enqueue(() => _redisBranchService.DelAllBranchById(getBranch.Data.SupplierProfilesId));
            BackgroundJob.Enqueue(() => _redisBranchService.DelBranchById(getBranch.Data.SupplierProfilesId));

            return Response<SupplierBranchDTO>.Success(result, "Update Branch successfully.");
        }
    }
}
