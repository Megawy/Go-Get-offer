using GoGetOffer.SharedLibrarySolution.DTOs.Auth.User.ConfirmEmail;
using GoGetOffer.SharedLibrarySolution.Responses;

namespace AuthenticationApi.Application.Services.Interfaces.AuthenticationService.EmailService
{
    public interface IUserEmailService
    {
        Task<Response<ConfirmEmailOtpDTO>> SendEmailOtpAsync(Guid Id);
        Task<Response<ConfirmEmailOtpDTO>> ConfirmEmailOtpAsync(Guid Id, string otp);
    }
}
