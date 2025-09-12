using AuthenticationApi.Domain.Entites.Supplier;
using AutoMapper;
using GoGetOffer.SharedLibrarySolution.DTOs.Supplier.JoinRequest;

namespace AuthenticationApi.Application.Mapping.SupplierMapping.Join
{
    public class JoinMapping : Profile
    {
        public JoinMapping()
        {
            CreateMap<SupplierJoinRequest, CreateJoinRequestDTO>().ReverseMap();
            CreateMap<ProfileJoinRequestDTO, SupplierJoinRequest>().ReverseMap();
        }
    }
}
