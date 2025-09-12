using GoGetOffer.SharedLibrarySolution.DTOs.Auth.User.UserProfile;

namespace GoGetOffer.SharedLibrarySolution.Interface.JWT
{
    public interface IJwtService
    {
        string GenerateAccessToken(UserDTO user);
        string GenerateRefreshToken(UserDTO user);
    }
}
