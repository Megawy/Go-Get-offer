using GoGetOffer.SharedLibrarySolution.DTOs.Auth.User.UpdateUser;
using GoGetOffer.SharedLibrarySolution.DTOs.Auth.User.UserProfile;
using GoGetOffer.SharedLibrarySolution.Responses;

namespace AuthenticationApi.Application.Services.Interfaces.AuthenticationService.ActionService
{
    public interface IUserActionService
    {
        Task<Response<UserDTO>> SoftDeleteUserAsync(Guid Id);
        Task<Response<UserDTO>> UnSoftDeleteUserAsync(Guid Id);
        Task<Response<UserDTO>> BanUserAsync(Guid Id);
        Task<Response<UserDTO>> UnbanUserAsync(Guid Id);
        Task<Response<UserDTO>> ChangeRole(ChangeRoleDTO dto);
    }
}
