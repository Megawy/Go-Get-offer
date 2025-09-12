using AuthenticationApi.Domain.Entites.Auth;
using GoGetOffer.SharedLibrarySolution.Interface;
using GoGetOffer.SharedLibrarySolution.Responses;

namespace AuthenticationApi.Infrastructure.Repositories.Interfaces.Authentication
{
    public interface IRequestUserUpdateRepository : IGenericInterface<AuthenticationUserUpdateRequest>
    {
        Task<Response<AuthenticationUserUpdateRequest>> GetLastPendingRequestByUserIdAsync(Guid id);
    }
}
