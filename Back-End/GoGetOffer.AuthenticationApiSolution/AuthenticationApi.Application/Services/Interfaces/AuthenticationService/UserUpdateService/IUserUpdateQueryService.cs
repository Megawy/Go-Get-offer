using GoGetOffer.SharedLibrarySolution.DTOs.Auth.User.UpdateUser;
using GoGetOffer.SharedLibrarySolution.Responses;

namespace AuthenticationApi.Application.Services.Interfaces.AuthenticationService.UserUpdateService
{
    public interface IUserUpdateQueryService
    {
        Task<Response<IEnumerable<GetRequestUserUpdateDTO>>> GetAllRequestPendingUpdateUser();
        Task<Response<GetRequestUserUpdateDTO>> GetRequestUserUpdateById(Guid Id);
        Task<Response<GetRequestUserUpdateDTO>> GetRequestUserUpdateByUserId(Guid Id);
    }
}
