using AuthenticationApi.Application.Services.Interfaces.AuthenticationService.ActionService;
using AuthenticationApi.Application.Services.Interfaces.AuthenticationService.AuthService;
using AuthenticationApi.Application.Services.Interfaces.AuthenticationService.QueryService;
using AuthenticationApi.Domain.Entites.Auth;
using AuthenticationApi.Infrastructure.Repositories.Interfaces.Authentication;
using GoGetOffer.SharedLibrarySolution.DTOs.Auth.User.UpdateUser;
using GoGetOffer.SharedLibrarySolution.DTOs.Auth.User.UserProfile;
using GoGetOffer.SharedLibrarySolution.Responses;
using Hangfire;

namespace AuthenticationApi.Application.Services.AuthenticationService.ActionService
{
    public class UserActionService
         : IUserActionService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRedisQueryService _redisService;
        private readonly IUserAuthService _userAuth;

        public UserActionService(
        IUserRepository userRepository,
        IRedisQueryService redisService,
        IUserAuthService userAuth)
        {
            _userRepository = userRepository;
            _redisService = redisService;
            _userAuth = userAuth;
        }

        public async Task<Response<UserDTO>> ChangeRole(ChangeRoleDTO dto)
        {
            if (!Enum.TryParse<UserType>(dto.RoleName, true, out var parsedRole))
                return Response<UserDTO>.Failure("Invalid role value.");

            var userResult = await
                _userRepository.FindByIdAsync(dto.Id);

            if (!userResult.Status || userResult.Data is null)
                return Response<UserDTO>.Failure("User not found.");

            var user = userResult.Data;

            if (user.UserType == parsedRole)
                return Response<UserDTO>.Failure("User already has this role.");

            user.UserType = parsedRole;
            var updateResult = await _userRepository.UpdateAsync(user);

            if (!updateResult.Status || updateResult.Data is null)
                return Response<UserDTO>.Failure("Failed to update user role.");

            BackgroundJob.Enqueue(() => _redisService.DelUserInfo(user.Id));
            BackgroundJob.Enqueue(() => _redisService.DelUsers());

            return Response<UserDTO>.Success("Role updated successfully.");
        }

        public async Task<Response<UserDTO>> SoftDeleteUserAsync(Guid Id)
        {
            var userResult = await
                _userRepository.FindByIdAsync(Id);

            if (!userResult.Status || userResult.Data is null)
                return Response<UserDTO>.Failure($"User: {Id} not found or already deleted.");

            var user = userResult.Data;

            user.IsDeleted = true;
            user.DeletedAt = DateTime.UtcNow;

            var updateResult = await
                _userRepository.UpdateAsync(user);

            if (!updateResult.Status || updateResult.Data is null)
                return Response<UserDTO>.Failure($"User: {Id} could not be deleted.");

            await _redisService.DelUserInfo(Id);

            BackgroundJob.Enqueue(() => _redisService.DelUserInfo(Id));
            BackgroundJob.Enqueue(() => _redisService.DelUsers());
            BackgroundJob.Enqueue(() => _userAuth.Logout(Id));

            return Response<UserDTO>.Success("User soft deleted successfully.");
        }

        public async Task<Response<UserDTO>> UnSoftDeleteUserAsync(Guid Id)
        {
            var user = await
                _userRepository.FindByIdAsync(Id, true);

            if (user.Data is null || !user.Status)
                return Response<UserDTO>.Failure($"User: {Id} not found.");

            if (!user.Data.IsDeleted)
                return Response<UserDTO>.Failure($"User: {Id} is not deleted.");

            user.Data.IsDeleted = false;
            user.Data.DeletedAt = null;

            var updateResult = await
                _userRepository.UpdateAsync(user.Data);

            if (!updateResult.Status || updateResult.Data is null)
                return Response<UserDTO>.Failure($"User: {Id} can't be restored.");

            BackgroundJob.Enqueue(() => _redisService.DelUserInfo(Id));
            BackgroundJob.Enqueue(() => _redisService.DelUsers());

            return Response<UserDTO>.Success("User restored successfully.");
        }

        public async Task<Response<UserDTO>> BanUserAsync(Guid Id)
        {
            return await UpdateUserBanStatus(Id, true, "ban");
        }

        public async Task<Response<UserDTO>> UnbanUserAsync(Guid Id)
        {
            return await UpdateUserBanStatus(Id, false, "unban");
        }

        private async Task<Response<UserDTO>> UpdateUserBanStatus(Guid id, bool banStatus, string action)
        {
            var userResult = await
                _userRepository.FindByIdAsync(id, true);

            if (!userResult.Status || userResult.Data is null)
                return Response<UserDTO>.Failure("User not found.");

            var user = userResult.Data;

            if (user.IsBanned == banStatus)
            {
                var statusMessage = banStatus ? "already banned" : "not banned";
                return Response<UserDTO>.Failure($"User is {statusMessage}.");
            }

            user.IsBanned = banStatus;

            var updateResult = await
                _userRepository.UpdateAsync(user);

            if (!updateResult.Status || updateResult.Data is null)
                return Response<UserDTO>.Failure($"Failed to {action} user.");

            BackgroundJob.Enqueue(() => _redisService.DelUserInfo(user.Id));
            BackgroundJob.Enqueue(() => _redisService.DelUsers());
            if (banStatus) BackgroundJob.Enqueue(() => _userAuth.Logout(id));

            var successMessage = banStatus ? "User has been banned." : "User has been unbanned.";

            return Response<UserDTO>.Success(successMessage);
        }
    }
}
