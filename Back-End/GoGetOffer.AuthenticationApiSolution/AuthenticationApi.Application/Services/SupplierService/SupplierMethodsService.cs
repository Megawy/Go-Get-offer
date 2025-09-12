using AuthenticationApi.Application.Services.Interfaces.SupplierService;
using AuthenticationApi.Domain.Entites.Supplier;
using AuthenticationApi.Infrastructure.Repositories.Interfaces.Supplier;
using GoGetOffer.SharedLibrarySolution.DTOs.Supplier.Branch;
using GoGetOffer.SharedLibrarySolution.DTOs.Supplier.Profile;
using GoGetOffer.SharedLibrarySolution.Interface.Helper;
using GoGetOffer.SharedLibrarySolution.Responses;

namespace AuthenticationApi.Application.Services.SupplierService
{
    public class SupplierMethodsService
        (ISupplierRepository supplierRepository,
        IHelperMethodService helperMethodService) : ISupplierMethodsService
    {
        private readonly ISupplierRepository _supplierRepository = supplierRepository;
        private readonly IHelperMethodService _helperMethodService = helperMethodService;

        public Response<RegisterSupplierDTO> CreateEncryptedRegisterProfile(RegisterSupplierDTO dto)
        {
            var result = new RegisterSupplierDTO
            {
                Id = dto.Id,
                FullName = _helperMethodService.EncryptStringSafe(dto.FullName ?? string.Empty),
                ActivityType = dto.ActivityType,
                BranchName = _helperMethodService.EncryptStringSafe(dto.BranchName ?? string.Empty),
                Governorate = _helperMethodService.EncryptStringSafe(dto.Governorate ?? string.Empty),
                City = _helperMethodService.EncryptStringSafe(dto.City ?? string.Empty),
                Area = _helperMethodService.EncryptStringSafe(dto.Area ?? string.Empty),
                AddressDetails = _helperMethodService.EncryptStringSafe(dto.AddressDetails ?? string.Empty),
                PostalCode = _helperMethodService.EncryptStringSafe(dto.PostalCode ?? string.Empty),
                PhoneNumbers = _helperMethodService.EncryptListSafe(dto.PhoneNumbers)
            };
            return Response<RegisterSupplierDTO>.Success(result, "");
        }

        public Response<UpdateSupplierDTO> CreateEncryptedUpdateProfile(UpdateSupplierDTO dto)
        {

            var result = new UpdateSupplierDTO
            {
                Id = dto.Id,
                FullName = _helperMethodService.EncryptStringSafe(dto.FullName ?? string.Empty),
                ActivityType = dto.ActivityType,
            };
            return Response<UpdateSupplierDTO>.Success(result, "Done");
        }

        public Response<SupplierProfile> DecryptImgs(SupplierProfile profile)
        {
            if (profile == null)
                return Response<SupplierProfile>.Failure("Profile is null");

            profile.TaxCardDocumentUrl = _helperMethodService.DecryptListSafe(profile.TaxCardDocumentUrl);
            profile.TaxCardDocumentPublicId = _helperMethodService.DecryptListSafe(profile.TaxCardDocumentPublicId);

            profile.CommercialRegistrationDocumentUrl = _helperMethodService.DecryptListSafe(profile.CommercialRegistrationDocumentUrl);
            profile.CommercialRegistrationDocumentPublicId = _helperMethodService.DecryptListSafe(profile.CommercialRegistrationDocumentPublicId);

            return Response<SupplierProfile>.Success(profile, $"Profile decrypted successfully at {DateTime.UtcNow}");
        }

        public Response<SupplierProfile> DecryptProfileData(SupplierProfile profile)
        {
            if (profile == null)
                return Response<SupplierProfile>.Failure("Profile is null");

            profile.FullName = _helperMethodService.DecryptStringSafe(profile.FullName);

            profile.TaxCardDocumentUrl = _helperMethodService.DecryptListSafe(profile.TaxCardDocumentUrl);
            profile.TaxCardDocumentPublicId = _helperMethodService.DecryptListSafe(profile.TaxCardDocumentPublicId);

            profile.CommercialRegistrationDocumentUrl = _helperMethodService.DecryptListSafe(profile.CommercialRegistrationDocumentUrl);
            profile.CommercialRegistrationDocumentPublicId = _helperMethodService.DecryptListSafe(profile.CommercialRegistrationDocumentPublicId);

            return Response<SupplierProfile>.Success(profile, $"Profile decrypted successfully at {DateTime.UtcNow}");
        }

        public Response<CreateBranchDTO> CreateEncryptedBranch(CreateBranchDTO dto)
        {
            var result = new CreateBranchDTO
            {
                SupplierProfilesId = dto.SupplierProfilesId,

                BranchName = _helperMethodService.EncryptStringSafe(dto.BranchName ?? string.Empty),

                Governorate = _helperMethodService.EncryptStringSafe(dto.Governorate ?? string.Empty),

                City = _helperMethodService.EncryptStringSafe(dto.City ?? string.Empty),

                Area = _helperMethodService.EncryptStringSafe(dto.Area ?? string.Empty),

                AddressDetails = _helperMethodService.EncryptStringSafe(dto.AddressDetails ?? string.Empty),

                PostalCode = _helperMethodService.EncryptStringSafe(dto.PostalCode ?? string.Empty),

                PhoneNumbers = _helperMethodService.EncryptListSafe(dto.PhoneNumbers)
            };
            return Response<CreateBranchDTO>.Success(result, "Create Encrypted Branch successfully.");
        }

        public Response<UpdateBranchDTO> UpdateEncryptedBranch(UpdateBranchDTO dto)
        {
            var result = new UpdateBranchDTO
            {
                Id = dto.Id,

                BranchName = _helperMethodService.EncryptStringSafe(dto.BranchName ?? string.Empty),

                Governorate = _helperMethodService.EncryptStringSafe(dto.Governorate ?? string.Empty),

                City = _helperMethodService.EncryptStringSafe(dto.City ?? string.Empty),

                Area = _helperMethodService.EncryptStringSafe(dto.Area ?? string.Empty),

                AddressDetails = _helperMethodService.EncryptStringSafe(dto.AddressDetails ?? string.Empty),

                PostalCode = _helperMethodService.EncryptStringSafe(dto.PostalCode ?? string.Empty),

                PhoneNumbers = _helperMethodService.EncryptListSafe(dto.PhoneNumbers)
            };
            return Response<UpdateBranchDTO>.Success(result, "Create Encrypted Branch successfully.");
        }

        public Response<SupplierBranch> DecryptBranceData(SupplierBranch profile)
        {
            if (profile == null)
                return Response<SupplierBranch>.Failure("Profile is null");

            profile.BranchName = _helperMethodService.DecryptStringSafe(profile.BranchName);
            profile.Governorate = _helperMethodService.DecryptStringSafe(profile.Governorate);
            profile.City = _helperMethodService.DecryptStringSafe(profile.City);
            profile.Area = _helperMethodService.DecryptStringSafe(profile.Area);
            profile.AddressDetails = _helperMethodService.DecryptStringSafe(profile.AddressDetails);
            profile.PostalCode = _helperMethodService.DecryptStringSafe(profile.PostalCode);
            profile.PhoneNumbers = _helperMethodService.DecryptListSafe(profile.PhoneNumbers);

            return Response<SupplierBranch>.Success(profile, $"Brance decrypted successfully at {DateTime.UtcNow}");
        }

        public async Task<Response<SupplierProfile>> GetProfileByCode(string code)
        {
            _ = code.ToUpper();
            var Result = await _supplierRepository.GetByAsync(x => x.Code!.Equals(code));

            return Result.Status ?
                 Response<SupplierProfile>.Success(Result.Data)
                : Response<SupplierProfile>.Success("");
        }

        public Response<SuppilerProfileUpdate> DecryptProfileUpdate(SuppilerProfileUpdate profile)
        {
            if (profile == null)
                return Response<SuppilerProfileUpdate>.Failure("Profile is null");

            profile.NewFullName = _helperMethodService.DecryptStringSafe(profile.NewFullName);
            profile.NewGovernorate = _helperMethodService.DecryptStringSafe(profile.NewGovernorate);
            profile.NewCity = _helperMethodService.DecryptStringSafe(profile.NewCity);
            profile.NewArea = _helperMethodService.DecryptStringSafe(profile.NewArea);
            profile.NewAddressDetails = _helperMethodService.DecryptStringSafe(profile.NewAddressDetails);
            profile.NewPostalCode = _helperMethodService.DecryptStringSafe(profile.NewPostalCode);
            return Response<SuppilerProfileUpdate>.Success(profile, $"Profile Request decrypted successfully at {DateTime.UtcNow}");
        }
    }
}

