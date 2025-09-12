using GoGetOffer.SharedLibrarySolution.DTOs.Auth.User.Login;
using GoGetOffer.SharedLibrarySolution.DTOs.Auth.User.UserProfile;
using GoGetOffer.SharedLibrarySolution.Responses;

namespace AuthenticationApi.Application.Services.Interfaces.AuthenticationService.AuthService
{
    public interface IRedisAuthService
    {
        Task<Response<string>> GetTokenLogin(Guid id);
        Task<Response<string>> SetTokenLogin(UserDTO userDTO, string token, TimeSpan timeSpan);
        Task<Response<TokenDTO>> RefreshTokenLogin(UserDTO userDTO, string refreshToken);
        Task<Response<string>> SetRefreshToken(string refreshToken);
        Task<Response<string>> DelTokenLogin(Guid id);
    }
}
