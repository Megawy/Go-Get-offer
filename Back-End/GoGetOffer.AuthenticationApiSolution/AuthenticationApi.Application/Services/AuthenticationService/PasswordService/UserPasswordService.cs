using AuthenticationApi.Application.Services.Interfaces.AuthenticationService;
using AuthenticationApi.Application.Services.Interfaces.AuthenticationService.AuthService;
using AuthenticationApi.Application.Services.Interfaces.AuthenticationService.PasswordService;
using AuthenticationApi.Infrastructure.Repositories.Interfaces.Authentication;
using GoGetOffer.SharedLibrarySolution.DTOs.Auth.User.Login;
using GoGetOffer.SharedLibrarySolution.DTOs.Auth.User.UserPassword;
using GoGetOffer.SharedLibrarySolution.DTOs.Auth.User.UserProfile;
using GoGetOffer.SharedLibrarySolution.Interface.Helper;
using GoGetOffer.SharedLibrarySolution.Responses;
using Hangfire;

namespace AuthenticationApi.Application.Services.AuthenticationService.PasswordService
{
    public class UserPasswordService
        (IUserRepository userRepository,
        IUserMethodsService userMethodsService,
         IHelperMethodService helperMethodService,
         IRedisPasswordService redisService,
         IUserAuthService userAuth) : IUserPasswordService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IUserMethodsService _userMethodsService = userMethodsService;
        private readonly IHelperMethodService _helperMethodService = helperMethodService;
        private readonly IRedisPasswordService _redisService = redisService;
        private readonly IUserAuthService _userAuth = userAuth;

        public async Task<Response<AccessTokenDTO>> ChangePasswordAsync(ChangePasswordDTO dto)
        {
            var userResult = await _userRepository.FindByIdAsync(dto.Id, false);
            if (!userResult.Status || userResult.Data is null)
                return Response<AccessTokenDTO>.Failure("User not found.");

            var user = userResult.Data;

            if (!BCrypt.Net.BCrypt.Verify(dto.OldPassword, user.PasswordHash))
                return Response<AccessTokenDTO>.Failure("Current password is incorrect.");

            if (BCrypt.Net.BCrypt.Verify(dto.NewPassword, user.PasswordHash))
                return Response<AccessTokenDTO>.Failure("New password cannot be the same as the current password.");


            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            var updateResult = await _userRepository.UpdatePassword(user);

            if (!updateResult.Status)
                return Response<AccessTokenDTO>.Failure("Password could not be changed.");

            await _userAuth.Logout(dto.Id);

            return Response<AccessTokenDTO>.Success("Password changed successfully.");
        }

        public async Task<Response<AccessTokenDTO>> ResetPasswordAsync(ResetPasswordDTO dto)
        {
            var encryptedEmail = _helperMethodService.EncryptStringSafe(_helperMethodService.Normalize(dto.Email));

            var savedCode = await _redisService.GetResetPassword(encryptedEmail);

            if (savedCode.Data is null || savedCode.Data != dto.ResetCode!)
                return Response<AccessTokenDTO>.Failure("Invalid or expired reset code.");

            var user = await _userMethodsService.GetUserByEmail(dto.Email!, false);
            if (user.Data is null)
                return Response<AccessTokenDTO>.Failure("User not found.");

            if (BCrypt.Net.BCrypt.Verify(dto.NewPassword, user.Data.PasswordHash))
                return Response<AccessTokenDTO>.Failure("New password cannot be the same as the current password.");

            user.Data.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            await _userRepository.UpdatePassword(user.Data);

            BackgroundJob.Enqueue(() => _redisService.DeleteResetPassword(encryptedEmail));

            await _userAuth.Logout(user.Data.Id);

            return Response<AccessTokenDTO>.Success("Password has been reset successfully.");
        }

        public async Task<Response<UserDTO>> ForgotPasswordAsync(string email)
        {
            var user = await _userMethodsService.GetUserByEmail(email, true);
            if (user.Data is null)
                return Response<UserDTO>.Failure("User not found.");

            var OTP = new Random().Next(100000, 999999).ToString();
            var setOtpResult = await _redisService.SetResetPassword(user.Data.Email!, OTP);

            if (!setOtpResult.Status)
                return Response<UserDTO>.Failure(setOtpResult.Message!);

            //   BackgroundJob.Enqueue(() => _mailService.SendResetPasswordEmail(email, resetCode));

            return Response<UserDTO>.Success($"Reset code sent successfully. Code: {OTP}"); ;
        }
    }
}
