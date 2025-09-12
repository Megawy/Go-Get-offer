using AuthenticationApi.Domain.Entites.Auth;
using AutoMapper;
using GoGetOffer.SharedLibrarySolution.DTOs.Auth.User.UserProfile;

namespace AuthenticationApi.Application.Mapping.UserMapping.UserProfile
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {


            CreateMap<AuthenticationUser, UserDTO>()
                .ForMember(dest => dest.UserType, opt => opt.MapFrom(src => src.UserType.ToString()));

            CreateMap<UserDTO, AuthenticationUser>()
           .ForMember(dest => dest.UserType, opt => opt.MapFrom(src => Enum.Parse<UserType>(src.UserType!)));
        }

    }
}
