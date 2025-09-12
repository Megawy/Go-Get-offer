using AuthenticationApi.Domain.Entites.Supplier;
using AutoMapper;
using GoGetOffer.SharedLibrarySolution.DTOs.Supplier.Branch;

namespace AuthenticationApi.Application.Mapping.SupplierMapping.Branch
{
    public class BranchMapping : Profile
    {
        public BranchMapping()
        {
            CreateMap<SupplierBranchDTO, SupplierBranch>().ReverseMap();
            CreateMap<SupplierBranch, CreateBranchDTO>().ReverseMap();
            CreateMap<SupplierBranch, UpdateBranchDTO>().ReverseMap();
        }
    }
}
