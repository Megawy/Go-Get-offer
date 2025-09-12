using GoGetOffer.SharedLibrarySolution.DTOs.Supplier.Profile;
using GoGetOffer.SharedLibrarySolution.Responses;

namespace AuthenticationApi.Application.Services.Interfaces.SupplierService.ProFileService
{
    public interface IRedisSupplierQueryService
    {
        Task<Response<byte[]>> GetProfileInfoById(Guid Id);
        Task<Response<string>> SetProfileInfoById(SupplierProfileDTO dto);
        Task<Response<string>> DelProfileInfoById(Guid Id);

        Task<Response<byte[]>> GetProfileInfoByCode(CodeSupplierDTO dto);
        Task<Response<string>> SetProfileInfoByCode(SupplierProfileDTO dto);
        Task<Response<string>> DelProfileInfoByCode(CodeSupplierDTO dto);

        Task<Response<byte[]>> GetAllProfileInfo();
        Task<Response<string>> SetAllProfileInfo(IEnumerable<SupplierProfileDTO> dto);
        Task<Response<string>> DelAllProfileInfo();
    }
}
