using AuthenticationApi.Application.Services.Interfaces.AuthenticationService;
using AuthenticationApi.Application.Services.Interfaces.AuthenticationService.QueryService;
using AuthenticationApi.Application.Services.Interfaces.AuthenticationService.UserUpdateService;
using AuthenticationApi.Domain.Entites.Auth;
using AuthenticationApi.Infrastructure.Repositories.Interfaces.Authentication;
using AutoMapper;
using GoGetOffer.SharedLibrarySolution.DTOs.Auth.User.UpdateUser;
using GoGetOffer.SharedLibrarySolution.DTOs.Auth.User.UserProfile;
using GoGetOffer.SharedLibrarySolution.Interface.Helper;
using GoGetOffer.SharedLibrarySolution.Responses;
using Hangfire;

namespace AuthenticationApi.Application.Services.AuthenticationService.UserUpdateService
{
    public class UserUpdateCommandService : IUserUpdateCommandService
    {
        private readonly IMapper _mapper;
        private readonly IRequestUserUpdateRepository _requestUserUpdate;
        private readonly IUserMethodsService _userMethodsService;
        private readonly IHelperMethodService _helperMethodService;
        private readonly IUserRepository _userRepository;
        private readonly IRedisQueryService _redisQuery;

        public UserUpdateCommandService(
            IUserMethodsService userMethodsService,
            IMapper mapper,
            IRequestUserUpdateRepository requestUser,
            IHelperMethodService helperMethodService,
            IRedisQueryService redisQuery,
            IUserRepository userRepository)
        {
            _userMethodsService = userMethodsService;
            _mapper = mapper;
            _requestUserUpdate = requestUser;
            _helperMethodService = helperMethodService;
            _redisQuery = redisQuery;
            _userRepository = userRepository;
        }

        public async Task<Response<GetRequestUserUpdateDTO>> RequestUserUpdateAsync(RequestUserUpdateDTO dto)
        {
            var userResponse = await
                _userRepository.FindUserNoTracking(dto.AuthenticationUserId!.Value);

            if (!userResponse.Status || userResponse.Data is null)
                return Response<GetRequestUserUpdateDTO>.Failure(userResponse.Message ?? "User not found.");

            var existingRequest = await
                _requestUserUpdate.GetLastPendingRequestByUserIdAsync(dto.AuthenticationUserId.Value);

            if (existingRequest.Data != null && existingRequest.Data.IsApproved == IsApproved.Pending)
                return Response<GetRequestUserUpdateDTO>.Failure("You already have a pending update request.");

            var decryptedUser = _userMethodsService.DecryptUserData(userResponse.Data);
            var userDto = _mapper.Map<UserDTO>(decryptedUser.Data);

            var validationResult = await
                ValidateUpdateRequest(dto, userDto);

            if (!validationResult.Status || validationResult.Data is null)
                return Response<GetRequestUserUpdateDTO>.Failure(validationResult.Message ?? "Can't Validate Update Request");

            var entity = new AuthenticationUserUpdateRequest
            {
                AuthenticationUserId = dto.AuthenticationUserId.Value,
                NewEmail = string.IsNullOrWhiteSpace(dto.NewEmail)
                    ? null
                    : _helperMethodService.EncryptStringSafe(_helperMethodService.Normalize(dto.NewEmail)),
                NewPhoneNumber = string.IsNullOrWhiteSpace(dto.NewPhoneNumber)
                    ? null
                    : _helperMethodService.EncryptStringSafe(_helperMethodService.Normalize(dto.NewPhoneNumber)),
                NewCompanyName = string.IsNullOrWhiteSpace(dto.NewCompanyName)
                    ? null
                    : _helperMethodService.EncryptStringSafe(_helperMethodService.Normalize(dto.NewCompanyName)),
                RequestedAt = DateTime.UtcNow,
                IsApproved = IsApproved.Pending,
                UserComment = dto.UserComment
            };

            var createRequest = await
                _requestUserUpdate.CreateAsync(entity);

            if (!createRequest.Status || createRequest.Data is null)
                return Response<GetRequestUserUpdateDTO>.Failure(createRequest.Message ?? "Can't Create Request Update.");

            return Response<GetRequestUserUpdateDTO>.Success("Update request submitted successfully.");
        }

        public async Task<Response<GetRequestUserUpdateDTO>> CancelRequestUserUpdateAsync(Guid Id)
        {
            var existingRequest = await
                _requestUserUpdate.GetLastPendingRequestByUserIdAsync(Id);

            if (!existingRequest.Status || existingRequest.Data == null)
                return Response<GetRequestUserUpdateDTO>.Failure("No pending request found to cancel.");

            if (existingRequest.Data.IsApproved != IsApproved.Pending)
                return Response<GetRequestUserUpdateDTO>.Failure("Only pending requests can be canceled.");

            var deleteResult = await
                _requestUserUpdate.DeleteAsync(existingRequest.Data);

            if (!deleteResult.Status || deleteResult.Data is null)
                return Response<GetRequestUserUpdateDTO>.Failure(deleteResult.Message ?? "Failed to cancel request.");

            return Response<GetRequestUserUpdateDTO>.Success("Request canceled successfully.");
        }

        private async Task<Response<string>> ValidateUpdateRequest(RequestUserUpdateDTO dto, UserDTO user)
        {
            string normalizedEmail = _helperMethodService.Normalize(dto.NewEmail ?? "");
            string normalizedPhone = _helperMethodService.Normalize(dto.NewPhoneNumber ?? "");
            string normalizedCompany = _helperMethodService.Normalize(dto.NewCompanyName ?? "");

            // Check if data is same as current
            bool isSameAsCurrent =
                (string.IsNullOrWhiteSpace(normalizedEmail) || normalizedEmail == user.Email) &&
                (string.IsNullOrWhiteSpace(normalizedPhone) || normalizedPhone == user.PhoneNumber) &&
                (string.IsNullOrWhiteSpace(normalizedCompany) || normalizedCompany == user.CompanyName);

            if (isSameAsCurrent)
                return Response<string>.Failure("The new data is the same as the current user data.");

            // Check for duplicates
            if (!string.IsNullOrWhiteSpace(normalizedEmail))
            {
                var existingUser = await _userMethodsService.GetUserByEmail(normalizedEmail, true);
                if (existingUser.Data != null && existingUser.Data!.Id != user.Id)
                    return Response<string>.Failure($"Email: {normalizedEmail} is already in use.");
            }

            if (!string.IsNullOrWhiteSpace(normalizedPhone))
            {
                var existingUser = await _userMethodsService.GetUserByPhoneNumber(normalizedPhone, true);
                if (existingUser.Data != null && existingUser.Data!.Id != user.Id)
                    return Response<string>.Failure($"PhoneNumber: {normalizedPhone} is already in use.");
            }

            if (!string.IsNullOrWhiteSpace(normalizedCompany))
            {
                var existingUser = await _userMethodsService.GetUserByCompanyName(normalizedCompany, true);
                if (existingUser.Data != null && existingUser.Data!.Id != user.Id)
                    return Response<string>.Failure($"CompanyName: {normalizedCompany} is already in use.");
            }

            return Response<string>.Success("Validation passed");
        }

        public async Task<Response<UserDTO>> ApproveUserUpdateAsync(ApproveUserUpdateDTO dto)
        {
            if (!Enum.TryParse<IsApproved>(dto.Status, true, out var parsedStatus))
                return Response<UserDTO>.Failure("Invalid Status value.");

            var requestResult = await _requestUserUpdate.FindByIdAsync(dto.Id);
            if (!requestResult.Status || requestResult.Data == null)
                return Response<UserDTO>.Failure("Update request not found.");

            var request = requestResult.Data;
            if (request.IsApproved != IsApproved.Pending)
                return Response<UserDTO>.Failure("This request has already been processed.");

            var userResult = await _userRepository.FindByIdAsync(request.AuthenticationUserId);
            if (!userResult.Status || userResult.Data is null)
                return Response<UserDTO>.Failure("User not found.");

            var user = userResult.Data;

            if (parsedStatus == IsApproved.Approved)
            {
                bool hasChanges = false;

                if (!string.IsNullOrWhiteSpace(request.NewEmail) && user.Email != request.NewEmail)
                {
                    user.Email = request.NewEmail;
                    user.IsEmailConfirmed = false;
                    hasChanges = true;
                }
                if (!string.IsNullOrWhiteSpace(request.NewCompanyName) && user.CompanyName != request.NewCompanyName)
                {
                    user.CompanyName = request.NewCompanyName;
                    hasChanges = true;
                }
                if (!string.IsNullOrWhiteSpace(request.NewPhoneNumber) && user.PhoneNumber != request.NewPhoneNumber)
                {
                    user.PhoneNumber = request.NewPhoneNumber;
                    hasChanges = true;
                }

                if (hasChanges)
                {
                    var updateResult = await _userRepository.UpdateAsync(user);
                    if (!updateResult.Status)
                        return Response<UserDTO>.Failure("Failed to apply approved changes.");

                    BackgroundJob.Enqueue(() => _redisQuery.DelUserInfo(user.Id));
                    BackgroundJob.Enqueue(() => _redisQuery.DelUsers());
                }
            }

            request.IsApproved = parsedStatus;
            request.DecisionAt = DateTime.UtcNow;
            request.AdminComment = dto.AdminComment;

            await _requestUserUpdate.UpdateAsync(request);

            var message = parsedStatus == IsApproved.Approved
                ? "Request approved and changes applied."
                : "Request rejected.";

            return Response<UserDTO>.Success(message);
        }
    }
}
