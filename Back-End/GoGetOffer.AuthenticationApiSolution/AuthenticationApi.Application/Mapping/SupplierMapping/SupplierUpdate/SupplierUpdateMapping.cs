using AuthenticationApi.Domain.Entites.Supplier;
using AutoMapper;
using GoGetOffer.SharedLibrarySolution.DTOs.Supplier.UpdateRequest;

namespace AuthenticationApi.Application.Mapping.SupplierMapping.SupplierUpdate;

public class SupplierUpdateMapping : Profile
{
    public SupplierUpdateMapping()
    {
        CreateMap<SuppilerProfileUpdate, SupplierUpdateProfileDTO>().ReverseMap();
    }
}

