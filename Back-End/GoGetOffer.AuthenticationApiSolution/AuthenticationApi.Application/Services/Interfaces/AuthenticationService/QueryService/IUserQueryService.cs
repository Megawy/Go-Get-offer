using GoGetOffer.SharedLibrarySolution.DTOs.Auth.User.UserProfile;
using GoGetOffer.SharedLibrarySolution.Responses;

namespace AuthenticationApi.Application.Services.Interfaces.AuthenticationService.QueryService
{
    public interface IUserQueryService
    {
        Task<Response<UserDTO>> RefreshUserCache(Guid Id, bool includeDeleted);
        Task<Response<UserDTO>> GetUserByIdAsync(Guid Id, bool includeDeleted = false);
        Task<Response<IEnumerable<UserDTO>>> RefreshUsersCache();
        Task<Response<IEnumerable<UserDTO>>> GetAllUsersAsync(bool includeDeleted = false);
    }
}
