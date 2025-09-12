using AuthenticationApi.Application.Services.Interfaces.SupplierService.JoinService;
using AuthenticationApi.Application.Services.Interfaces.SupplierService.ProFileService;
using AuthenticationApi.Application.Services.Interfaces.SupplierService.ProfileUpdateService;
using AuthenticationApi.Domain.Entites.Supplier;
using AuthenticationApi.Infrastructure.Repositories.Interfaces.Supplier;
using GoGetOffer.SharedLibrarySolution.DTOs.Supplier.UpdateRequest;
using GoGetOffer.SharedLibrarySolution.Interface.Helper;
using GoGetOffer.SharedLibrarySolution.Responses;
using Hangfire;

namespace AuthenticationApi.Application.Services.SupplierService.ProfileUpdateService
{
    public class SupplierUpdateCommandService : ISupplierUpdateCommandService
    {
        private readonly IProfileSupplierUpdateRepository _profileSupplierUpdateRepository;
        private readonly ISupplierRepository _supplierRepository;
        private readonly ISupplierBranchRepository _supplierBranch;
        private readonly IHelperMethodService _helperMethod;
        private readonly IBackgroundJobClient _jobs;
        public SupplierUpdateCommandService(
            IProfileSupplierUpdateRepository profileSupplierUpdateRepository,
            ISupplierRepository supplierRepository,
            IHelperMethodService helperMethod, IBackgroundJobClient jobs,
            ISupplierBranchRepository supplierBranch)
        {
            _profileSupplierUpdateRepository = profileSupplierUpdateRepository;
            _supplierRepository = supplierRepository;
            _helperMethod = helperMethod;
            _jobs = jobs;
            _supplierBranch = supplierBranch;
        }

        public async Task<Response<SupplierUpdateProfileDTO>> ApproveSupplierUpdate(ApproveSupplierUpdateDTO dto)
        {
            if (!Enum.TryParse<IsApproved>(dto.Status, true, out var parsedStatus))
                return Response<SupplierUpdateProfileDTO>.Failure("Invalid Status value.");

            var requestResult = await
                 _profileSupplierUpdateRepository.FindByIdAsync(dto.Id);

            if (!requestResult.Status || requestResult.Data is null)
                return Response<SupplierUpdateProfileDTO>.Failure(requestResult.Message ?? "Update Request not found");

            var request = requestResult.Data;

            if (request.IsApproved != Domain.Entites.Supplier.IsApproved.Pending)
                return Response<SupplierUpdateProfileDTO>.Failure("This request has already been processed.");

            var profileResult = await
                _supplierRepository.FindByIdAsync(request.SupplierProfilesId);

            if (!profileResult.Status || profileResult.Data is null)
                return Response<SupplierUpdateProfileDTO>.Failure(requestResult.Message ?? "Supplier not found");

            var profile = profileResult.Data;

            var branchResult = await
                _supplierBranch.GetAllAsync();
            if (!branchResult.Status || branchResult.Data is null)
                return Response<SupplierUpdateProfileDTO>.Failure(requestResult.Message ?? "Branch not found");

            var branch = branchResult.Data
                    .FirstOrDefault(x => x.SupplierProfilesId == request.SupplierProfilesId && x.Main_Branch == true);

            if (branch is null)
                return Response<SupplierUpdateProfileDTO>.Failure("Main branch not found");

            if (parsedStatus == IsApproved.Approved)
                await ApplyApprovedChanges(request, profile, branch);

            request.IsApproved = parsedStatus;
            request.DecisionAt = DateTime.UtcNow;
            request.AdminComment = dto.AdminComment;

            await _profileSupplierUpdateRepository.UpdateAsync(request);

            InvalidateSupplierCache(request.SupplierProfilesId);

            var message = parsedStatus
                == IsApproved.Approved
                ? "Request approved and changes applied."
                : "Request rejected.";

            return Response<SupplierUpdateProfileDTO>.Success(message);
        }

        public async Task<Response<SupplierUpdateProfileDTO>> CancelRequestUserUpdateByUserId(Guid Id)
        {
            var getSupId = await
                _supplierRepository.FindByIdAsync(Id);
            if (!getSupId.Status || getSupId.Data is null)
                return Response<SupplierUpdateProfileDTO>.Failure(getSupId.Message ?? "Not found supplier to cancel request.");

            var existingRequest = await
                _profileSupplierUpdateRepository.GetLastPendingRequestByUserId(getSupId.Data.Id);
            if (!existingRequest.Status || existingRequest.Data == null)
                return Response<SupplierUpdateProfileDTO>.Failure("No pending request found to cancel.");

            if (existingRequest.Data.IsApproved != Domain.Entites.Supplier.IsApproved.Pending)
                return Response<SupplierUpdateProfileDTO>.Failure("Only pending requests can be canceled.");

            var delete = await
                _profileSupplierUpdateRepository.DeleteAsync(existingRequest.Data);
            if (!delete.Status || delete.Data is null)
                return Response<SupplierUpdateProfileDTO>.Failure(delete.Message ?? "Failed to cancel request.");

            InvalidateSupplierCache(getSupId.Data.Id);

            return Response<SupplierUpdateProfileDTO>.Success("Request canceled successfully.");
        }

        public async Task<Response<SupplierUpdateProfileDTO>> MakeRequestUserUpdate(CreateRequestSupplierUpdateProfileDTO dto)
        {
            var getSupId = await
                _supplierRepository.FindByIdAsync(dto.UserId);

            if (!getSupId.Status || getSupId.Data is null)
                return Response<SupplierUpdateProfileDTO>.Failure(getSupId.Message ?? "Not found supplier to cancel request.");

            var existingRequest = await
                _profileSupplierUpdateRepository.GetLastPendingRequestByUserId(getSupId.Data.Id);

            if (existingRequest.Data != null && existingRequest.Data.IsApproved == Domain.Entites.Supplier.IsApproved.Pending)
                return Response<SupplierUpdateProfileDTO>.Failure("You already have a pending update request.");

            var entity = new SuppilerProfileUpdate
            {
                SupplierProfilesId = getSupId.Data.Id,
                NewFullName = _helperMethod.EncryptStringSafe(dto.NewFullName),
                NewGovernorate = _helperMethod.EncryptStringSafe(dto.NewGovernorate),
                NewCity = _helperMethod.EncryptStringSafe(dto.NewCity),
                NewArea = _helperMethod.EncryptStringSafe(dto.NewArea),
                NewAddressDetails = _helperMethod.EncryptStringSafe(dto.NewAddressDetails),
                NewPostalCode = _helperMethod.EncryptStringSafe(dto.NewPostalCode),
                UserComment = dto.UserComment,
                RequestedAt = DateTime.UtcNow,
                IsApproved = Domain.Entites.Supplier.IsApproved.Pending
            };

            if (dto.NewFullName is null)
            {
                entity.NewFullName = null;
            }
            if (dto.NewGovernorate is null)
            {
                entity.NewGovernorate = null;
            }
            if (dto.NewCity is null)
            {
                entity.NewCity = null;
            }
            if (dto.NewArea is null)
            {
                entity.NewArea = null;
            }
            if (dto.NewAddressDetails is null)
            {
                entity.NewAddressDetails = null;
            }
            if (dto.NewPostalCode is null)
            {
                entity.NewPostalCode = null;
            }

            var createRequest = await
                _profileSupplierUpdateRepository.CreateAsync(entity);
            if (!createRequest.Status || createRequest.Data is null)
                return Response<SupplierUpdateProfileDTO>.Failure(createRequest.Message ?? "Update failed.");

            InvalidateSupplierCache(getSupId.Data.Id);

            return Response<SupplierUpdateProfileDTO>.Success("Update request submitted successfully.");
        }

        private async Task<Response<string>> ApplyApprovedChanges(SuppilerProfileUpdate request, SupplierProfile profile, SupplierBranch branch)
        {
            bool hasChanges = false;

            if (!string.IsNullOrWhiteSpace(request.NewFullName) && profile.FullName != request.NewFullName)
            {
                profile.FullName = request.NewFullName;
                hasChanges = true;
            }

            if (!string.IsNullOrWhiteSpace(request.NewGovernorate) && branch.Governorate != request.NewGovernorate)
            {
                branch.Governorate = request.NewGovernorate;
                hasChanges = true;
            }

            if (!string.IsNullOrWhiteSpace(request.NewCity) && branch.City != request.NewCity)
            {
                branch.City = request.NewCity;
                hasChanges = true;
            }

            if (!string.IsNullOrWhiteSpace(request.NewArea) && branch.Area != request.NewArea)
            {
                branch.Area = request.NewArea;
                hasChanges = true;
            }

            if (!string.IsNullOrWhiteSpace(request.NewAddressDetails) && branch.AddressDetails != request.NewAddressDetails)
            {
                branch.AddressDetails = request.NewAddressDetails;
                hasChanges = true;
            }

            if (!string.IsNullOrWhiteSpace(request.NewPostalCode) && branch.PostalCode != request.NewPostalCode)
            {
                branch.PostalCode = request.NewPostalCode;
                hasChanges = true;
            }

            if (hasChanges)
            {
                var updateSupplier = await _supplierRepository.UpdateAsync(profile);
                if (!updateSupplier.Status)
                    throw new InvalidOperationException("Failed to apply approved changes supplier.");

                var updatebranch = await
                    _supplierBranch.UpdateAsync(branch);
                if (!updatebranch.Status)
                    throw new InvalidOperationException("Failed to apply approved changes branch.");
            }
            return Response<string>.Success("Done Update");
        }

        private void InvalidateSupplierCache(Guid profileId)
        {
            _jobs.Enqueue<IRedisSupplierUpdateService>(s => s.DelRequestById(profileId));
            _jobs.Enqueue<IRedisSupplierUpdateService>(s => s.DelAllRequestUpdate());
            _jobs.Enqueue<IRedisSupplierQueryService>(s => s.DelAllProfileInfo());
            _jobs.Enqueue<IRedisJoinService>(s => s.DelAllRequestJoinInfo());
        }
    }
}
