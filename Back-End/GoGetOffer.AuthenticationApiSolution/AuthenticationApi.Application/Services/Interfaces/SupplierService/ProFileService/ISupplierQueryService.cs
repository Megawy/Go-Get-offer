using GoGetOffer.SharedLibrarySolution.DTOs.Supplier.Profile;
using GoGetOffer.SharedLibrarySolution.Responses;

namespace AuthenticationApi.Application.Services.Interfaces.SupplierService.ProFileService
{
    public interface ISupplierQueryService
    {
        Task<Response<SupplierProfileDTO>> GetProfileById(Guid id);
        Task<Response<SupplierProfileDTO>> RefreshProfileById(Guid id);

        Task<Response<SupplierProfileDTO>> GetProfileByCode(CodeSupplierDTO dto);
        Task<Response<SupplierProfileDTO>> RefreshProfileByCode(CodeSupplierDTO dto);

        Task<Response<IEnumerable<SupplierProfileDTO>>> GetProfiles();
        Task<Response<IEnumerable<SupplierProfileDTO>>> RefreshProfiles();
    }
}
