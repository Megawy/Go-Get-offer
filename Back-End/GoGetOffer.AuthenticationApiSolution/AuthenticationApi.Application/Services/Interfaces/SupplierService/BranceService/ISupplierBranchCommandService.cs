using GoGetOffer.SharedLibrarySolution.DTOs.Supplier.Branch;
using GoGetOffer.SharedLibrarySolution.Responses;

namespace AuthenticationApi.Application.Services.Interfaces.SupplierService.BranceService
{
    public interface ISupplierBranchCommandService
    {
        Task<Response<SupplierBranchDTO>> CreateBranch(CreateBranchDTO dto);
        Task<Response<SupplierBranchDTO>> UpdateBranch(UpdateBranchDTO dto);
        Task<Response<SupplierBranchDTO>> DeleteBranch(Guid id);
    }
}
