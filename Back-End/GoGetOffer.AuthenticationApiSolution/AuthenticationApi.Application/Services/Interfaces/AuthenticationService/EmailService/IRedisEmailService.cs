using GoGetOffer.SharedLibrarySolution.Responses;

namespace AuthenticationApi.Application.Services.Interfaces.AuthenticationService.EmailService
{
    public interface IRedisEmailService
    {
        Task<Response<string>> GetOTP(Guid id);
        Task<Response<string>> SetOTP(Guid id, string otp);
        Task<Response<string>> DelOTP(Guid id);
    }
}
