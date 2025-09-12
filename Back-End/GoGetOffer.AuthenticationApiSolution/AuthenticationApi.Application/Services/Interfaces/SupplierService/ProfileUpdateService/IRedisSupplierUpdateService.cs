using GoGetOffer.SharedLibrarySolution.DTOs.Supplier.UpdateRequest;
using GoGetOffer.SharedLibrarySolution.Responses;

namespace AuthenticationApi.Application.Services.Interfaces.SupplierService.ProfileUpdateService
{
    public interface IRedisSupplierUpdateService
    {
        Task<Response<string>> SetRequestById(SupplierUpdateProfileDTO dto);
        Task<Response<byte[]>> GetRequestById(Guid id);
        Task<Response<string>> DelRequestById(Guid id);

        Task<Response<byte[]>> GetAllRequestUpdate();
        Task<Response<string>> SetAllRequestUpdate(IEnumerable<SupplierUpdateProfileDTO> dto);
        Task<Response<string>> DelAllRequestUpdate();
    }
}
