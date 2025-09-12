using GoGetOffer.SharedLibrarySolution.Responses;

namespace AuthenticationApi.Application.Services.Interfaces.AuthenticationService.PasswordService
{
    public interface IRedisPasswordService
    {
        Task<Response<string>> SetResetPassword(string email, string resetCode);
        Task<Response<string>> GetResetPassword(string email);
        Task<Response<string>> DeleteResetPassword(string email);
    }
}
