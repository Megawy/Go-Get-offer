using GoGetOffer.SharedLibrarySolution.DTOs.Supplier.UpdateRequest;
using GoGetOffer.SharedLibrarySolution.Responses;

namespace AuthenticationApi.Application.Services.Interfaces.SupplierService.ProfileUpdateService
{
    public interface ISupplierUpdateCommandService
    {
        Task<Response<SupplierUpdateProfileDTO>> CancelRequestUserUpdateByUserId(Guid Id);
        Task<Response<SupplierUpdateProfileDTO>> MakeRequestUserUpdate(CreateRequestSupplierUpdateProfileDTO dto);
        Task<Response<SupplierUpdateProfileDTO>> ApproveSupplierUpdate(ApproveSupplierUpdateDTO dto);
    }
}
