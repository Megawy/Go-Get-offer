using AuthenticationApi.Domain.Entites.Supplier;
using GoGetOffer.SharedLibrarySolution.DTOs.Supplier.Branch;
using GoGetOffer.SharedLibrarySolution.DTOs.Supplier.Profile;
using GoGetOffer.SharedLibrarySolution.Responses;

namespace AuthenticationApi.Application.Services.Interfaces.SupplierService
{
    public interface ISupplierMethodsService
    {
        Task<Response<SupplierProfile>> GetProfileByCode(string code);
        Response<RegisterSupplierDTO> CreateEncryptedRegisterProfile(RegisterSupplierDTO dto);
        Response<UpdateSupplierDTO> CreateEncryptedUpdateProfile(UpdateSupplierDTO dto);
        Response<SupplierProfile> DecryptImgs(SupplierProfile profile);
        Response<CreateBranchDTO> CreateEncryptedBranch(CreateBranchDTO dto);
        Response<UpdateBranchDTO> UpdateEncryptedBranch(UpdateBranchDTO dto);
        Response<SupplierProfile> DecryptProfileData(SupplierProfile profile);
        Response<SupplierBranch> DecryptBranceData(SupplierBranch profile);
        Response<SuppilerProfileUpdate> DecryptProfileUpdate(SuppilerProfileUpdate profile);
    }
}
