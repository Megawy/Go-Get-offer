using GoGetOffer.SharedLibrarySolution.DTOs.Supplier.JoinRequest;
using GoGetOffer.SharedLibrarySolution.Responses;

namespace AuthenticationApi.Application.Services.Interfaces.SupplierService.JoinService
{
    public interface ISupplierJoinQueryService
    {
        Task<Response<ProfileJoinRequestDTO>> GetRequestById(Guid id);
        Task<Response<IEnumerable<ProfileJoinRequestDTO>>> GetAllRequestPending();
        Task<Response<IEnumerable<ProfileJoinRequestDTO>>> RefreshGetAllRequestPending();
    }
}
