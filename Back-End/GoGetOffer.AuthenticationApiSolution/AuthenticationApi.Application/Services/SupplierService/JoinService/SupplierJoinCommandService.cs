using AuthenticationApi.Application.Services.Interfaces.SupplierService.JoinService;
using AuthenticationApi.Application.Services.Interfaces.SupplierService.ProFileService;
using AuthenticationApi.Domain.Entites.Auth;
using AuthenticationApi.Domain.Entites.Supplier;
using AuthenticationApi.Infrastructure.Repositories.Interfaces.Authentication;
using AuthenticationApi.Infrastructure.Repositories.Interfaces.Supplier;
using GoGetOffer.SharedLibrarySolution.DTOs.Supplier.JoinRequest;
using GoGetOffer.SharedLibrarySolution.Responses;
using Hangfire;

namespace AuthenticationApi.Application.Services.SupplierService.JoinService
{
    public class SupplierJoinCommandService
        (ISupplierJoinRequestRepository supplierJoin,
        ISupplierRepository supplierRepository,
        IUserRepository userRepository,
        IRedisJoinService redisJoin,
        IRedisSupplierQueryService redisSupplierQuery) : ISupplierJoinCommandService
    {
        private readonly ISupplierJoinRequestRepository _supplierJoin = supplierJoin;
        private readonly ISupplierRepository _supplierRepository = supplierRepository;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IRedisJoinService _redisJoin = redisJoin;
        private readonly IRedisSupplierQueryService _redisSupplierQuery = redisSupplierQuery;


        public async Task<Response<ProfileJoinRequestDTO>> ReplyRequestJoin(ReplyRequestJoinDTO dto)
        {
                var requestResult = await _supplierJoin.FindByIdAsync(dto.Id);
                if (!requestResult.Status || requestResult.Data == null)
                    return Response<ProfileJoinRequestDTO>.Failure("request not found.");

                var request = requestResult.Data;

                if (request.IsApproved != IsApproved_Join.Pending)
                    return Response<ProfileJoinRequestDTO>.Failure("This request has already been processed.");

                var profileResult = await _supplierRepository.FindByIdAsync(request.SupplierProfilesId);
                if (!profileResult.Status || profileResult.Data == null)
                    return Response<ProfileJoinRequestDTO>.Failure("profile not found.");

                var profile = profileResult.Data;

                var getUser = await _userRepository.FindByIdAsync(profile.Id);
                if (!getUser.Status || getUser.Data == null)
                    return Response<ProfileJoinRequestDTO>.Failure("user not found.");

                var isApprovedEnum = Enum.Parse<IsApproved_Join>(dto.IsApproved!, true);

                request.IsApproved = isApprovedEnum;
                request.DecisionAt = DateTime.UtcNow;
                request.AdminComment = dto.AdminComment;
                await _supplierJoin.UpdateAsync(request);

                var userEntity = getUser.Data;
                var ProfileEntity = profile;

                if (isApprovedEnum == IsApproved_Join.Approved)
                {
                    await ApplyApprovedChanges(userEntity, ProfileEntity);
                }

                if (isApprovedEnum == IsApproved_Join.Rejected)
                {
                    await ApplyRejectedChanges(ProfileEntity);
                }

                BackgroundJob.Enqueue(() => _redisJoin.DelAllRequestJoinInfo());
                BackgroundJob.Enqueue(() => _redisSupplierQuery.DelAllProfileInfo());
                BackgroundJob.Enqueue(() => _redisJoin.DelRequestByIdJoinInfo(profile.Id));
                BackgroundJob.Enqueue(() => _redisSupplierQuery.DelProfileInfoById(profile.Id));

                var message = isApprovedEnum == IsApproved_Join.Approved
                    ? "Request approved and changes applied."
                    : "Request rejected.";
                return Response<ProfileJoinRequestDTO>.Success(message);
        }

        private async Task<Response<string>> ApplyApprovedChanges(AuthenticationUser user, SupplierProfile profile)
        {
                // User
                user.UserType = UserType.Supplier;
                user.IsStatusConfirmed = true;

                var updateResultuser = await _userRepository.UpdateAsync(user);
                if (!updateResultuser.Status)
                    return Response<string>.Failure("Failed to update user after approval.");

                // Supplier 
                profile.Status = SupplierStatus.Approved;
                profile.UpdatedAt = DateTime.UtcNow;

                var updateResult = await _supplierRepository.UpdateAsync(profile);
                if (!updateResult.Status)
                    return Response<string>.Failure("Failed to update profile after approval.");

                return Response<string>.Success("Profile updated successfully.");
        }

        private async Task<Response<string>> ApplyRejectedChanges(SupplierProfile profile)
        {
                profile.Status = SupplierStatus.Rejected;
                profile.UpdatedAt = DateTime.UtcNow;

                var updateResult = await _supplierRepository.UpdateAsync(profile);
                if (!updateResult.Status)
                    return Response<string>.Failure("Failed to update profile after approval.");

                return Response<string>.Success("Profile updated successfully.");
        }
    }
}
