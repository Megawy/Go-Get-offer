using GoGetOffer.SharedLibrarySolution.DTOs.Supplier.JoinRequest;
using GoGetOffer.SharedLibrarySolution.Responses;

namespace AuthenticationApi.Application.Services.Interfaces.SupplierService.JoinService
{
    public interface IRedisJoinService
    {
        Task<Response<byte[]>> GetAllRequestJoinInfo();
        Task<Response<string>> SetAllRequestJoinInfo(IEnumerable<ProfileJoinRequestDTO> dto);
        Task<Response<string>> DelAllRequestJoinInfo();

        Task<Response<byte[]>> GetRequestByIdJoinInfo(Guid id);
        Task<Response<string>> SetRequestByIdJoinInfo(ProfileJoinRequestDTO dto);
        Task<Response<string>> DelRequestByIdJoinInfo(Guid id);
    }
}
