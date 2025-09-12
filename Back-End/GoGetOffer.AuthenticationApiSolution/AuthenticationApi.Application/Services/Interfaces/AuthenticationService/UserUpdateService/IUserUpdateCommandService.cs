using GoGetOffer.SharedLibrarySolution.DTOs.Auth.User.UpdateUser;
using GoGetOffer.SharedLibrarySolution.DTOs.Auth.User.UserProfile;
using GoGetOffer.SharedLibrarySolution.Responses;

namespace AuthenticationApi.Application.Services.Interfaces.AuthenticationService.UserUpdateService
{
    public interface IUserUpdateCommandService
    {
        Task<Response<UserDTO>> ApproveUserUpdateAsync(ApproveUserUpdateDTO dto);
        Task<Response<GetRequestUserUpdateDTO>> RequestUserUpdateAsync(RequestUserUpdateDTO dto);
        Task<Response<GetRequestUserUpdateDTO>> CancelRequestUserUpdateAsync(Guid Id);
    }
}
