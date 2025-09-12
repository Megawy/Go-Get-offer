using GoGetOffer.SharedLibrarySolution.DTOs.Supplier.Branch;
using GoGetOffer.SharedLibrarySolution.DTOs.Supplier.Profile;
using GoGetOffer.SharedLibrarySolution.Responses;

namespace AuthenticationApi.Application.Services.Interfaces.SupplierService.BranceService
{
    public interface ISupplierBranceQueryService
    {
        Task<Response<IEnumerable<SupplierBranchDTO>>> GetAllBranchByUserId(Guid id);
        Task<Response<SupplierBranchDTO>> GetBranchById(Guid id);
        Task<Response<IEnumerable<SupplierBranchDTO>>> GetBranchByCode(CodeSupplierDTO dto);
    }
}
