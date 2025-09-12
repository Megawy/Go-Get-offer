using AuthenticationApi.Domain.Entites.Supplier;
using AutoMapper;
using GoGetOffer.SharedLibrarySolution.DTOs.Supplier.Profile;

namespace AuthenticationApi.Application.Mapping.SupplierMapping.ProfileSupplier
{
    public class SupplierMapping : Profile
    {
        public SupplierMapping()
        {
            CreateMap<SupplierProfile, RegisterSupplierDTO>().ReverseMap();
            CreateMap<SupplierProfileDTO, SupplierProfile>().ReverseMap();
            CreateMap<UpdateSupplierDTO, SupplierProfile>().ReverseMap();
            CreateMap<AddSomeDataSupplierDTO, SupplierProfile>().ReverseMap();
        }
    }
}

