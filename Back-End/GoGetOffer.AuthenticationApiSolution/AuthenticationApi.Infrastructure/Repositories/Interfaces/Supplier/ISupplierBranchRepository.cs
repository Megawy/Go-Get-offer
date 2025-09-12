using AuthenticationApi.Domain.Entites.Supplier;
using GoGetOffer.SharedLibrarySolution.DTOs.Supplier.Profile;
using GoGetOffer.SharedLibrarySolution.Interface;
using GoGetOffer.SharedLibrarySolution.Responses;

namespace AuthenticationApi.Infrastructure.Repositories.Interfaces.Supplier
{
    public interface ISupplierBranchRepository : IGenericInterface<SupplierBranch>
    {
        Task<Response<IEnumerable<SupplierBranch>>> GetAllBranchByUserId(Guid id);
        Task<Response<IEnumerable<SupplierBranch>>> GetAllBranchByUserCode(CodeSupplierDTO dto);
    }
}
