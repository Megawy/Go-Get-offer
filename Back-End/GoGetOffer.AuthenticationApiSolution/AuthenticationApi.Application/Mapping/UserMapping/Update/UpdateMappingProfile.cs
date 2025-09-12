using AuthenticationApi.Domain.Entites.Auth;
using AutoMapper;
using GoGetOffer.SharedLibrarySolution.DTOs.Auth.User.UpdateUser;

namespace AuthenticationApi.Application.Mapping.UserMapping.Update
{
    public class UpdateMappingProfile : Profile
    {
        public UpdateMappingProfile()
        {
            CreateMap<AuthenticationUserUpdateRequest, GetRequestUserUpdateDTO>()
                .ForMember(dest => dest.IsApproved, opt => opt.MapFrom(src => src.IsApproved.ToString())).ReverseMap();
            CreateMap<AuthenticationUserUpdateRequest, RequestUserUpdateDTO>().ReverseMap();
        }
    }
}
