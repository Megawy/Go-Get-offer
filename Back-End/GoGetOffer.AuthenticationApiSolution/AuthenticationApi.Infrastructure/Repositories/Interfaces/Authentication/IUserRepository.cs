using AuthenticationApi.Domain.Entites.Auth;
using GoGetOffer.SharedLibrarySolution.Interface;
using GoGetOffer.SharedLibrarySolution.Responses;

namespace AuthenticationApi.Infrastructure.Repositories.Interfaces.Authentication
{
    public interface IUserRepository : IGenericInterface<AuthenticationUser>
    {
        Task<Response<AuthenticationUser>> UpdatePassword(AuthenticationUser entity);
        Task<Response<AuthenticationUser>> UpdateSpecificPropertiesAsync(Guid id, Action<AuthenticationUser> updateAction);
        Task<bool> ExistsAsync(Guid id, bool includeDeleted = false);
    }
}
