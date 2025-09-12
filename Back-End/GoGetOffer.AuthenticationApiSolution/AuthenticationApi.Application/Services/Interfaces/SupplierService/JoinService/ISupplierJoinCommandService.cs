using GoGetOffer.SharedLibrarySolution.DTOs.Supplier.JoinRequest;
using GoGetOffer.SharedLibrarySolution.Responses;

namespace AuthenticationApi.Application.Services.Interfaces.SupplierService.JoinService
{
    public interface ISupplierJoinCommandService
    {
        Task<Response<ProfileJoinRequestDTO>> ReplyRequestJoin(ReplyRequestJoinDTO dto);
    }
}
