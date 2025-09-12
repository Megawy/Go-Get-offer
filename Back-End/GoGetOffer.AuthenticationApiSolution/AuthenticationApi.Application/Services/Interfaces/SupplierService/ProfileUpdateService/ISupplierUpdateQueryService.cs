using GoGetOffer.SharedLibrarySolution.DTOs.Supplier.UpdateRequest;
using GoGetOffer.SharedLibrarySolution.Responses;

namespace AuthenticationApi.Application.Services.Interfaces.SupplierService.ProfileUpdateService
{
    public interface ISupplierUpdateQueryService
    {
        Task<Response<SupplierUpdateProfileDTO>> GetRequestById(Guid id);
        Task<Response<IEnumerable<SupplierUpdateProfileDTO>>> GetAllRequestPending();
    }
}
