using GoGetOffer.SharedLibrarySolution.DTOs.Supplier.Branch;
using GoGetOffer.SharedLibrarySolution.Responses;

namespace AuthenticationApi.Application.Services.Interfaces.SupplierService.BranceService
{
    public interface IRedisBranchService
    {
        Task<Response<byte[]>> GetAllBranchById(Guid id);
        Task<Response<string>> SetAllBranchById(Guid id, IEnumerable<SupplierBranchDTO> dto);
        Task<Response<string>> DelAllBranchById(Guid id);

        Task<Response<byte[]>> GetBranchById(Guid id);
        Task<Response<string>> SetBranchById(Guid id,SupplierBranchDTO dto);
        Task<Response<string>> DelBranchById(Guid id);
    }
}
