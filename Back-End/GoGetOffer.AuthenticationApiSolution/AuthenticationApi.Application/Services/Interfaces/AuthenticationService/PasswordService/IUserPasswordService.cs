using GoGetOffer.SharedLibrarySolution.DTOs.Auth.User.Login;
using GoGetOffer.SharedLibrarySolution.DTOs.Auth.User.UserPassword;
using GoGetOffer.SharedLibrarySolution.DTOs.Auth.User.UserProfile;
using GoGetOffer.SharedLibrarySolution.Responses;

namespace AuthenticationApi.Application.Services.Interfaces.AuthenticationService.PasswordService
{
    public interface IUserPasswordService
    {
        Task<Response<AccessTokenDTO>> ChangePasswordAsync(ChangePasswordDTO dto);
        Task<Response<AccessTokenDTO>> ResetPasswordAsync(ResetPasswordDTO dto);
        Task<Response<UserDTO>> ForgotPasswordAsync(string email);
    }
}
