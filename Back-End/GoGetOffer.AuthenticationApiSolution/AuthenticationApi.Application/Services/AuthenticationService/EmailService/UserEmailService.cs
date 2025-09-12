using AuthenticationApi.Application.Services.Interfaces;
using AuthenticationApi.Application.Services.Interfaces.AuthenticationService;
using AuthenticationApi.Application.Services.Interfaces.AuthenticationService.EmailService;
using AuthenticationApi.Application.Services.Interfaces.AuthenticationService.QueryService;
using AuthenticationApi.Infrastructure.Repositories.Interfaces.Authentication;
using GoGetOffer.SharedLibrarySolution.DTOs.Auth.User.ConfirmEmail;
using GoGetOffer.SharedLibrarySolution.Responses;
using Hangfire;

namespace AuthenticationApi.Application.Services.AuthenticationService.EmailService
{
    public class UserEmailService
        (IUserRepository userRepository,
        IRedisEmailService redisService,
        IRedisQueryService redisQueryService)
        : IUserEmailService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IRedisEmailService _redisService = redisService;
        private readonly IRedisQueryService _redisQueryService = redisQueryService;

        public async Task<Response<ConfirmEmailOtpDTO>> SendEmailOtpAsync(Guid Id)
        {
            var userResult = await _userRepository.FindByIdAsync(Id, false);
            if (!userResult.Status || userResult.Data == null)
                return Response<ConfirmEmailOtpDTO>.Failure("User not found.");

            var user = userResult.Data;

            if (user.IsEmailConfirmed!)
                return Response<ConfirmEmailOtpDTO>.Failure("Email is already Confirmed.");

            var OTP = new Random().Next(100000, 999999).ToString();

            var SetOTP = await _redisService.SetOTP(Id, OTP);
            if (!SetOTP.Status)
                return Response<ConfirmEmailOtpDTO>.Failure(SetOTP.Message ?? "Can't Send OTP.");

            // BackgroundJob.Enqueue(() => _emailService.SendEmailAsync(email, "Email Confirmation OTP", $"Your OTP is: {OTP}"));

            var result = new ConfirmEmailOtpDTO
            {
                Id = Id,
                Otp = OTP,
            };

            return Response<ConfirmEmailOtpDTO>.Success(result, "OTP sent successfully.");
        }

        public async Task<Response<ConfirmEmailOtpDTO>> ConfirmEmailOtpAsync(Guid Id, string otp)
        {
            if (string.IsNullOrWhiteSpace(otp))
                return Response<ConfirmEmailOtpDTO>.Failure("Invalid parameters.");

            var userResult = await _userRepository.FindByIdAsync(Id);
            if (!userResult.Status || userResult.Data == null)
                return Response<ConfirmEmailOtpDTO>.Failure("User not found.");

            var user = userResult.Data;
            if (user.IsEmailConfirmed)
                return Response<ConfirmEmailOtpDTO>.Failure("Email is already Confirmed.");

            var getOtpResult = await _redisService.GetOTP(Id);
            if (!getOtpResult.Status || string.IsNullOrEmpty(getOtpResult.Data))
                return Response<ConfirmEmailOtpDTO>.Failure("OTP not found or expired.");

            if (getOtpResult.Data != otp)
                return Response<ConfirmEmailOtpDTO>.Failure("Invalid OTP.");

            user.IsEmailConfirmed = true;
            await _userRepository.UpdateAsync(user);

            BackgroundJob.Enqueue(() => _redisService.DelOTP(Id));
            BackgroundJob.Enqueue(() => _redisQueryService.DelUserInfo(Id));
            BackgroundJob.Enqueue(() => _redisQueryService.DelUsers());

            return Response<ConfirmEmailOtpDTO>.Success("Email confirmed successfully.");
        }
    }
}
