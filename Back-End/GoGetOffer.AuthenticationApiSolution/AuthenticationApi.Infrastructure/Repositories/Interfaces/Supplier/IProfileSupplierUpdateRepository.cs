using AuthenticationApi.Domain.Entites.Supplier;
using GoGetOffer.SharedLibrarySolution.Interface;
using GoGetOffer.SharedLibrarySolution.Responses;

namespace AuthenticationApi.Infrastructure.Repositories.Interfaces.Supplier
{
    public interface IProfileSupplierUpdateRepository : IGenericInterface<SuppilerProfileUpdate>
    {
        Task<Response<SuppilerProfileUpdate>> GetLastPendingRequestByUserId(Guid id);
    }
}
