using GoGetOffer.SharedLibrarySolution.DTOs.Auth.User.Login;
using GoGetOffer.SharedLibrarySolution.DTOs.Auth.User.Register;
using GoGetOffer.SharedLibrarySolution.DTOs.Auth.User.UserProfile;
using GoGetOffer.SharedLibrarySolution.Responses;

namespace AuthenticationApi.Application.Services.Interfaces.AuthenticationService.AuthService
{
    public interface IUserAuthService
    {
        Task<Response<AccessTokenDTO>> RegisterAsync(RegisterUserDTO dto);
        Task<Response<AccessTokenDTO>> LoginAsync(LoginDTO dto);
        Task<Response<AccessTokenDTO>> RefreshToken(Guid id);
        Task<Response<UserDTO>> Logout(Guid Id);
    }
}
