using AuthenticationApi.Domain.Entites.Auth;
using AutoMapper;
using GoGetOffer.SharedLibrarySolution.DTOs.Auth.User.Login;
using GoGetOffer.SharedLibrarySolution.DTOs.Auth.User.Register;

namespace AuthenticationApi.Application.Mapping.UserMapping.Register___Login
{
    public class LoginMappingProfile : Profile
    {
        public LoginMappingProfile()
        {
            CreateMap<AuthenticationUser, RegisterUserDTO>().ReverseMap();
            CreateMap<AuthenticationUser, LoginDTO>().ReverseMap();
        }
    }
}
