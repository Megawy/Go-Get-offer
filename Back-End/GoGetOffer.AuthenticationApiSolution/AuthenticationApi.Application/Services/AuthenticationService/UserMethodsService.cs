using AuthenticationApi.Application.Services.Interfaces.AuthenticationService;
using AuthenticationApi.Domain.Entites.Auth;
using AuthenticationApi.Infrastructure.Repositories.Interfaces.Authentication;
using GoGetOffer.SharedLibrarySolution.Interface.Helper;
using GoGetOffer.SharedLibrarySolution.Responses;

namespace AuthenticationApi.Application.Services.AuthenticationService
{
    public class UserMethodsService
        (IUserRepository userRepository,
         IHelperMethodService helperMethodService) : IUserMethodsService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IHelperMethodService _helperMethodService = helperMethodService;

        public async Task<Response<AuthenticationUser?>> GetUserByEmail(string email, bool includeDeleted = false)
        {

            var encryptedEmail = _helperMethodService.EncryptStringSafe(_helperMethodService.Normalize(email));
            var userResult = await _userRepository.GetByAsync(x => x.Email!.Equals(encryptedEmail), includeDeleted);

            return userResult.Status ?
                 Response<AuthenticationUser?>.Success(userResult.Data)
                : Response<AuthenticationUser?>.Success("");
        }

        public async Task<Response<AuthenticationUser?>> GetUserByPhoneNumber(string phoneNumber, bool includeDeleted = false)
        {

            var encryptedPhone = _helperMethodService.EncryptStringSafe(_helperMethodService.Normalize(phoneNumber));
            var userResult = await _userRepository.GetByAsync(x => x.PhoneNumber!.Equals(encryptedPhone), includeDeleted);

            return userResult.Status ?
                Response<AuthenticationUser?>.Success(userResult.Data)
                : Response<AuthenticationUser?>.Success("");
        }

        public async Task<Response<AuthenticationUser?>> GetUserByCompanyName(string companyName, bool includeDeleted = false)
        {

            var encryptedCompanyName = _helperMethodService.EncryptStringSafe(_helperMethodService.Normalize(companyName));
            var userResult = await _userRepository.GetByAsync(x => x.CompanyName!.Equals(encryptedCompanyName), includeDeleted);

            return userResult.Status ?
                   Response<AuthenticationUser?>.Success(userResult.Data)
                   : Response<AuthenticationUser?>.Success("");
        }

        public Response<AuthenticationUser> DecryptUserData(AuthenticationUser user)
        {
            if (user == null)
                return Response<AuthenticationUser>.Failure("User is Null");

            user.Email = _helperMethodService.DecryptStringSafe(user.Email);
            user.PhoneNumber = _helperMethodService.DecryptStringSafe(user.PhoneNumber);
            user.CompanyName = _helperMethodService.DecryptStringSafe(user.CompanyName);
            return Response<AuthenticationUser>.Success(user, "Validation passed");
        }

        public Response<AuthenticationUserUpdateRequest> DecryptRequestData(AuthenticationUserUpdateRequest request)
        {
            if (request == null)
                return Response<AuthenticationUserUpdateRequest>.Failure("request is Null");

            request.NewEmail = _helperMethodService.DecryptStringSafe(request.NewEmail);
            request.NewPhoneNumber = _helperMethodService.DecryptStringSafe(request.NewPhoneNumber);
            request.NewCompanyName = _helperMethodService.DecryptStringSafe(request.NewCompanyName);

            return Response<AuthenticationUserUpdateRequest>.Success(request, "Request decrypted successfully");
        }
    }
}
