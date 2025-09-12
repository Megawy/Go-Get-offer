using AuthenticationApi.Domain.Entites.Auth;
using AuthenticationApi.Domain.Entites.Supplier;
using GoGetOffer.SharedLibrarySolution.Interface;
using GoGetOffer.SharedLibrarySolution.Responses;

namespace AuthenticationApi.Infrastructure.Repositories.Interfaces.Supplier
{
    public interface ISupplierJoinRequestRepository : IGenericInterface<SupplierJoinRequest>
    {
        Task<Response<SupplierJoinRequest>> GetLastPendingRequestByUserIdAsync(Guid id);
    }
}
