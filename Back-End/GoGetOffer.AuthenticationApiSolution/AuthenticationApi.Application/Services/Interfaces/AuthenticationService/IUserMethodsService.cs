using AuthenticationApi.Domain.Entites.Auth;
using GoGetOffer.SharedLibrarySolution.Responses;

namespace AuthenticationApi.Application.Services.Interfaces.AuthenticationService
{
    public interface IUserMethodsService
    {
        Task<Response<AuthenticationUser?>> GetUserByEmail(string email, bool includeDeleted = false);
        Task<Response<AuthenticationUser?>> GetUserByPhoneNumber(string phoneNumber, bool includeDeleted = false);
        Task<Response<AuthenticationUser?>> GetUserByCompanyName(string companyName, bool includeDeleted = false);
        Response<AuthenticationUser> DecryptUserData(AuthenticationUser user);
        Response<AuthenticationUserUpdateRequest> DecryptRequestData(AuthenticationUserUpdateRequest request);
    }
}
