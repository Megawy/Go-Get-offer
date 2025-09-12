using AuthenticationApi.Application.Services.Interfaces.SupplierService;
using AuthenticationApi.Application.Services.Interfaces.SupplierService.JoinService;
using AuthenticationApi.Application.Services.Interfaces.SupplierService.ProFileService;
using AuthenticationApi.Domain.Entites.Supplier;
using AuthenticationApi.Infrastructure.Repositories.Interfaces.Supplier;
using Hangfire;
using AutoMapper;
using Pipelines.Sockets.Unofficial.Arenas;
using GoGetOffer.SharedLibrarySolution.DTOs.Supplier.Profile;
using GoGetOffer.SharedLibrarySolution.Responses;
using GoGetOffer.SharedLibrarySolution.DTOs.Supplier.Branch;
using GoGetOffer.SharedLibrarySolution.DTOs.Supplier.JoinRequest;
using GoGetOffer.SharedLibrarySolution.Interface;

namespace AuthenticationApi.Application.Services.SupplierService.ProFileService
{
    public class SupplierCommandService : ISupplierCommandService
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly ISupplierBranchRepository _supplierBranch;
        private readonly ISupplierJoinRequestRepository _supplierJoin;
        private readonly ISupplierMethodsService _supplierMethods;
        private readonly IMapper _mapper;
        private readonly IBackgroundJobClient _jobs;
        private readonly IValidateUploadAndEncrypt _uploadAndEncrypt;

        public SupplierCommandService(
          ISupplierRepository supplierRepository,
          ISupplierBranchRepository supplierBranch,
          ISupplierJoinRequestRepository supplierJoin,
          ISupplierMethodsService supplierMethods,
          IMapper mapper,
          IBackgroundJobClient jobs,
          IValidateUploadAndEncrypt uploadAndEncrypt)
        {
            _supplierRepository = supplierRepository;
            _supplierBranch = supplierBranch;
            _supplierJoin = supplierJoin;
            _supplierMethods = supplierMethods;
            _mapper = mapper;
            _jobs = jobs;
            _uploadAndEncrypt = uploadAndEncrypt;
        }

        public async Task<Response<AddSomeDataSupplierDTO>> AddSomeDataSupplier(AddSomeDataSupplierDTO dto)
        {
            var profileEntity = await
                _supplierRepository.FindByIdAsync(dto.Id!.Value);

            if (profileEntity is null || !profileEntity.Status)
                return Response<AddSomeDataSupplierDTO>.Failure("Profile not found.");


            // ✅ Update supplier info
            profileEntity.Data!.MinProducts = dto.MinProducts;
            profileEntity.Data!.MaxProducts = dto.MaxProducts;
            profileEntity.Data!.DeliveryTimeInDays = dto.DeliveryTimeInDays;
            profileEntity.Data!.MinInvoiceAmount = dto.MinInvoiceAmount;
            profileEntity.Data!.HasElctroinInvoice = dto.HasElctroinInvoice;
            profileEntity.Data!.HasDeliveryService = dto.HasDeliveryService;
            profileEntity.Data!.UpdatedAt = DateTime.UtcNow;

            var updated = await
                _supplierRepository.UpdateAsync(profileEntity.Data!);

            if (!updated.Status) return
                    Response<AddSomeDataSupplierDTO>.Failure("Failed to update supplier profile in DB.");

            InvalidateSupplierCache(profileEntity.Data.Id);

            return Response<AddSomeDataSupplierDTO>.Success("Update data is Successfuly.");
        }

        public async Task<Response<SupplierProfileDTO>> DeleteProfile(Guid id)
        {
            var getProfile = await _supplierRepository.FindByIdAsync(id);
            if (!getProfile.Status || getProfile.Data is null)
                return Response<SupplierProfileDTO>.Failure(getProfile.Message!);


            var del = await _supplierRepository.DeleteAsync(getProfile.Data!);
            if (!del.Status || del.Data is null)
                return Response<SupplierProfileDTO>.Failure(del.Message!);

            InvalidateSupplierCache(getProfile.Data.Id);

            return Response<SupplierProfileDTO>.Success("Delete is Successfuly.");
        }

        public async Task<Response<SupplierProfileDTO>> RegisterProfile(RegisterSupplierDTO dto)
        {
            var existing = await
                _supplierRepository.FindByIdAsync(dto.Id);

            if (existing.Data is not null)
            {
                var decExisting = _supplierMethods.DecryptProfileData(existing.Data);
                var dtoProfile = _mapper.Map<SupplierProfileDTO>(decExisting.Data);
                return Response<SupplierProfileDTO>.Failure(dtoProfile, "You have a already profile.");
            }

            var encReg = _supplierMethods.CreateEncryptedRegisterProfile(dto);
            var profileEntity = _mapper.Map<SupplierProfile>(encReg.Data);

            // ✅ Commercial Docs
            if (dto.CommercialRegistrationDocuments == null || !dto.CommercialRegistrationDocuments.Any())
                return Response<SupplierProfileDTO>.Failure("Commercial Registration Documents are required.");

            var (okCom, errCom, comItems) =
                await _uploadAndEncrypt.ValidateUploadAndEncryptAsync(dto.CommercialRegistrationDocuments,
                    $"Users/Suppliers/commercial_docs/{dto.Id}");
            if (!okCom) return Response<SupplierProfileDTO>.Failure(errCom!);

            profileEntity.CommercialRegistrationDocumentUrl = comItems.Select(i => i.url).ToList();
            profileEntity.CommercialRegistrationDocumentPublicId = comItems.Select(i => i.publicId).ToList();

            if (dto.TaxCardDocuments is null || !dto.TaxCardDocuments.Any())
                return Response<SupplierProfileDTO>.Failure("Tax Card Documents are required.");

            var (okTax, errTax, taxItems) =
                await _uploadAndEncrypt.ValidateUploadAndEncryptAsync(dto.TaxCardDocuments,
                    $"Users/Suppliers/tax_docs/{dto.Id}");
            if (!okTax) return Response<SupplierProfileDTO>.Failure(errTax!);

            profileEntity.TaxCardDocumentUrl = taxItems.Select(i => i.url).ToList();
            profileEntity.TaxCardDocumentPublicId = taxItems.Select(i => i.publicId).ToList();

            // ✅ Generate unique supplier code
            string newCode;
            do
            {
                newCode = GenerateSupplierCode();
            }
            while ((await _supplierRepository.GetByAsync(x => x.Code == newCode)).Data is not null);
            profileEntity.Code = newCode;

            // ✅ Save supplier profile
            var createdProfile = await _supplierRepository.CreateAsync(profileEntity);
            if (!createdProfile.Status) return Response<SupplierProfileDTO>.Failure("Can't create profile in DB");


            // ✅ Create main branch
            var createBranch = new CreateBranchDTO
            {
                SupplierProfilesId = createdProfile.Data!.Id,
                BranchName = encReg.Data!.BranchName,
                Governorate = encReg.Data!.Governorate,
                City = encReg.Data!.City,
                Area = encReg.Data!.Area,
                AddressDetails = encReg.Data!.AddressDetails,
                PostalCode = encReg.Data!.PostalCode,
                PhoneNumbers = encReg.Data!.PhoneNumbers
            };

            var branchEntity = _mapper.Map<SupplierBranch>(createBranch);
            branchEntity.Main_Branch = true;

            var createdBranch = await _supplierBranch.CreateAsync(branchEntity);
            if (!createdBranch.Status) return Response<SupplierProfileDTO>.Failure("Can't create branch in DB");

            // ✅ Create join request
            var createJoinRequest = new CreateJoinRequestDTO
            {
                SupplierProfilesId = createdProfile.Data!.Id,
            };

            var joinEntity = _mapper.Map<SupplierJoinRequest>(createJoinRequest);

            var createdJoin = await _supplierJoin.CreateAsync(joinEntity);
            if (!createdJoin.Status)
                return Response<SupplierProfileDTO>.Failure($"Can't Create request join in DB");

            var dec = _supplierMethods.DecryptProfileData(createdProfile.Data);
            var result = _mapper.Map<SupplierProfileDTO>(dec.Data);

            InvalidateSupplierCache(createdProfile.Data!.Id);

            return Response<SupplierProfileDTO>.Success(result, "Supplier profile registered successfully.");
        }

        public async Task<Response<UpdateSupplierDTO>> UpdateDataSupplier(UpdateSupplierDTO dto)
        {
            // ✅ Commercial Docs
            if (dto.CommercialRegistrationDocuments != null)
            {
                {
                    var check = await _uploadAndEncrypt.ValidateUploadAndEncryptAsync(dto.CommercialRegistrationDocuments,
                        $"Users/Suppliers/commercial_docs/{dto.Id}");
                    if (!check.ok) return Response<UpdateSupplierDTO>.Failure(check.error!);
                }
            }

            // ✅ Tax Card Docs  
            if (dto.TaxCardDocuments != null)
            {
                {
                    var check = await _uploadAndEncrypt.ValidateUploadAndEncryptAsync(dto.TaxCardDocuments,
                        $"Users/Suppliers/tax_docs/{dto.Id}");
                    if (!check.ok) return Response<UpdateSupplierDTO>.Failure(check.error!);
                }
            }

            var getProfile = await
                _supplierRepository.FindByIdAsync(dto.Id);

            if (!getProfile.Status ||
                getProfile.Status && getProfile.Data!.Status != (SupplierStatus?)IsApproved.Rejected)
                return Response<UpdateSupplierDTO>.Failure("Profile is not in Rejected state.");

            var profileEntity = getProfile.Data;

            if (!string.IsNullOrWhiteSpace(dto.FullName))
            {
                var enc = _supplierMethods.CreateEncryptedUpdateProfile(dto);
                if (!enc.Status) return
                    Response<UpdateSupplierDTO>.Failure("Failed to encrypt update payload.");
                profileEntity!.FullName = enc.Data!.FullName;
            }

            // ✅ Upload Commercial Docs (if any)
            if (dto.CommercialRegistrationDocuments is not null && dto.CommercialRegistrationDocuments.Any())
            {
                var (okCom, errCom, comItems) = await _uploadAndEncrypt.ValidateUploadAndEncryptAsync(
                    dto.CommercialRegistrationDocuments, $"Users/Suppliers/commercial_docs/{dto.Id}");
                if (!okCom) return Response<UpdateSupplierDTO>.Failure(errCom!);

                profileEntity!.CommercialRegistrationDocumentUrl = comItems.Select(i => i.url).ToList();
                profileEntity.CommercialRegistrationDocumentPublicId = comItems.Select(i => i.publicId).ToList();
            }

            // ✅ Upload Tax Card Docs (if any)
            if (dto.TaxCardDocuments is not null && dto.TaxCardDocuments.Any())
            {
                var (okTax, errTax, taxItems) = await _uploadAndEncrypt.ValidateUploadAndEncryptAsync(
                    dto.TaxCardDocuments, $"Users/Suppliers/tax_docs/{dto.Id}");
                if (!okTax) return Response<UpdateSupplierDTO>.Failure(errTax!);

                profileEntity!.TaxCardDocumentUrl = taxItems.Select(i => i.url).ToList();
                profileEntity.TaxCardDocumentPublicId = taxItems.Select(i => i.publicId).ToList();
            }

            // ✅ Update supplier info
            profileEntity!.UpdatedAt = DateTime.UtcNow;
            profileEntity.Status = (SupplierStatus?)IsApproved.Pending;

            var updated = await _supplierRepository.UpdateAsync(profileEntity);

            if (!updated.Status) return
                    Response<UpdateSupplierDTO>.Failure("Failed to update supplier profile in DB.");

            // ✅ Create join request
            var createJoinRequest = new CreateJoinRequestDTO
            {
                SupplierProfilesId = getProfile.Data!.Id,
            };
            var joinEntity = _mapper.Map<SupplierJoinRequest>(createJoinRequest);

            var createdJoin = await _supplierJoin.CreateAsync(joinEntity);
            if (!createdJoin.Status)
                return Response<UpdateSupplierDTO>.Failure($"Can't Create request join in DB");

            InvalidateSupplierCache(getProfile.Data.Id);
            return Response<UpdateSupplierDTO>.Success("Update data is Successfuly.");
        }

        private static string GenerateSupplierCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var buffer = new char[8];
            for (int i = 0; i < buffer.Length; i++)
            {
                var idx = System.Security.Cryptography.RandomNumberGenerator.GetInt32(chars.Length);
                buffer[i] = chars[idx];
            }
            return $"S-{new string(buffer)}".ToUpperInvariant();
        }

        private void InvalidateSupplierCache(Guid profileId)
        {
            _jobs.Enqueue<IRedisSupplierQueryService>(s => s.DelProfileInfoById(profileId));
            _jobs.Enqueue<IRedisSupplierQueryService>(s => s.DelAllProfileInfo());
            _jobs.Enqueue<IRedisJoinService>(s => s.DelAllRequestJoinInfo());
        }
    }
}
