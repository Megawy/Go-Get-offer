using GoGetOffer.SharedLibrarySolution.DTOs.Auth.User.UserProfile;
using GoGetOffer.SharedLibrarySolution.Responses;

namespace AuthenticationApi.Application.Services.Interfaces.AuthenticationService.QueryService
{
    public interface IRedisQueryService
    {
        Task<Response<byte[]>> GetUserInfo(Guid Id);
        Task<Response<string>> SetUserInfo(UserDTO userDTO);
        Task<Response<string>> DelUserInfo(Guid id);

        Task<Response<byte[]>> GetAllUser();
        Task<Response<string>> SetAllUser(IEnumerable<UserDTO> users);
        Task<Response<string>> DelUsers();
    }
}
