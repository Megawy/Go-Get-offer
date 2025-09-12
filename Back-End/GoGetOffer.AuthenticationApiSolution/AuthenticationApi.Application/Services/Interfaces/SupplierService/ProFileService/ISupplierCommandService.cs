using GoGetOffer.SharedLibrarySolution.DTOs.Supplier.Profile;
using GoGetOffer.SharedLibrarySolution.Responses;

namespace AuthenticationApi.Application.Services.Interfaces.SupplierService.ProFileService
{
    public interface ISupplierCommandService
    {
        Task<Response<SupplierProfileDTO>> RegisterProfile(RegisterSupplierDTO dto);
        Task<Response<UpdateSupplierDTO>> UpdateDataSupplier(UpdateSupplierDTO dto);
        Task<Response<SupplierProfileDTO>> DeleteProfile(Guid id);
        Task<Response<AddSomeDataSupplierDTO>> AddSomeDataSupplier(AddSomeDataSupplierDTO dto);
    }
}
