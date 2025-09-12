using AuthenticationApi.Application.Services.Interfaces.AuthenticationService;
using AuthenticationApi.Application.Services.Interfaces.AuthenticationService.AuthService;
using AuthenticationApi.Application.Services.Interfaces.AuthenticationService.QueryService;
using AuthenticationApi.Infrastructure.Repositories.Interfaces.Authentication;
using AuthenticationApi.Domain.Entites.Auth;
using AutoMapper;
using Hangfire;
using Microsoft.AspNetCore.Http;
using GoGetOffer.SharedLibrarySolution.DTOs.Auth.User.Login;
using GoGetOffer.SharedLibrarySolution.DTOs.Auth.User.Register;
using GoGetOffer.SharedLibrarySolution.DTOs.Auth.User.UserProfile;
using GoGetOffer.SharedLibrarySolution.Responses;
using GoGetOffer.SharedLibrarySolution.Interface.Helper;
using GoGetOffer.SharedLibrarySolution.Interface.JWT;

namespace AuthenticationApi.Application.Services.AuthenticationService.AuthService
{
    public class UserAuthService
        (IUserMethodsService userMethodsService,
        IUserRepository userRepository,
        IHelperMethodService helperMethodService,
        IRedisAuthService redisService,
        IMapper mapper,
        IJwtService jwtService,
        IHttpContextAccessor httpContextAccessor,
        IRedisQueryService redisQuery) : IUserAuthService
    {
        private readonly IUserMethodsService _userMethodsService = userMethodsService;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IRedisAuthService _redisService = redisService;
        private readonly IMapper _mapper = mapper;
        private readonly IJwtService _jwtService = jwtService;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IRedisQueryService _redisQuery = redisQuery;
        private readonly IHelperMethodService _helperMethodService = helperMethodService;


        public async Task<Response<AccessTokenDTO>> RegisterAsync(RegisterUserDTO dto)
        {
            // Check for existing users
            var duplicateCheck = await CheckForDuplicateUser(dto.Email!, dto.PhoneNumber!, dto.CompanyName!);
            if (!duplicateCheck.Status)
                return Response<AccessTokenDTO>.Failure(duplicateCheck.Message!);

            var newUser = CreateNewUser(dto);

            var creationResult = await _userRepository.CreateAsync(newUser.Data!);
            if (!creationResult.Status || creationResult.Data is null)
                return Response<AccessTokenDTO>.Failure(creationResult.Message!);

            BackgroundJob.Enqueue(() => _redisQuery.DelUsers());

            var userMap = _mapper.Map<AuthenticationUser>(creationResult.Data);
            return GenerateLoginTokens(userMap);
        }

        public async Task<Response<AccessTokenDTO>> LoginAsync(LoginDTO dto)
        {
            // Check Mail in DB
            var user = await _userMethodsService.GetUserByEmail(dto.Email!, true);
            if (user.Data is null)
                return Response<AccessTokenDTO>.Failure($"Email: {dto.Email} is not registered.");

            // validation Pass and ban or deactive 
            if (user.Data.IsDeleted)
                return Response<AccessTokenDTO>.Failure("Your account has been deactive.");

            if (user.Data.IsBanned)
                return Response<AccessTokenDTO>.Failure("Your account has been banned.");

            if (!BCrypt.Net.BCrypt.Verify(dto.PasswordHash, user.Data.PasswordHash))
                return Response<AccessTokenDTO>.Failure("Invalid credentials.");

            return GenerateLoginTokens(user.Data!);
        }

        public async Task<Response<AccessTokenDTO>> RefreshToken(Guid id)
        {
            var userResponse = await _userRepository.FindByIdAsync(id);
            if (!userResponse.Status || userResponse.Data is null)
                return Response<AccessTokenDTO>.Failure("User not found.");

            var decryptedUser = _userMethodsService.DecryptUserData(userResponse.Data);
            var userDto = _mapper.Map<UserDTO>(decryptedUser.Data);

            var refreshResult = await _redisService.RefreshTokenLogin(userDto, id.ToString());
            if (!refreshResult.Status || refreshResult.Data is null)
                return Response<AccessTokenDTO>.Failure(refreshResult.Message!);

            var setToken = await _redisService.SetTokenLogin(userDto, refreshResult.Data.AccessToken!, TimeSpan.FromDays(7));
            if (!setToken.Status)
                return Response<AccessTokenDTO>.Failure("Can't set token login.");

            if (_httpContextAccessor.HttpContext == null)
                return Response<AccessTokenDTO>.Failure("No active HttpContext, cannot clear cookies.");

            var refreshCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(10),
                Path = "/"
            };

            _httpContextAccessor.HttpContext!.Response.Cookies.Append("RefreshToken", id.ToString()!, refreshCookieOptions);

            var result = new AccessTokenDTO
            {
                AccessToken = refreshResult.Data.AccessToken,
                IsEmailConfirmed = userResponse.Data.IsEmailConfirmed,
                IsStatusConfirmed = userResponse.Data.IsStatusConfirmed
            };

            return Response<AccessTokenDTO>.Success(result, refreshResult.Message!);
        }

        public async Task<Response<UserDTO>> Logout(Guid Id)
        {
            var getToken = await _redisService.GetTokenLogin(Id);
            if (!getToken.Status || string.IsNullOrEmpty(getToken.Data))
                return Response<UserDTO>.Failure("User is not logged in or token not found.");

            var delToken = await _redisService.DelTokenLogin(Id);
            if (!delToken.Status)
                return Response<UserDTO>.Failure("User is not logged in or token not found.");

            if (_httpContextAccessor.HttpContext == null)
                return Response<UserDTO>.Failure("No active HttpContext, cannot clear cookies.");

            var expiredCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(-1)
            };

            _httpContextAccessor.HttpContext.Response.Cookies.Append("RefreshToken", "", expiredCookieOptions);

            BackgroundJob.Enqueue(() => _redisQuery.DelUserInfo(Id));

            return Response<UserDTO>.Success("Logged out successfully.");
        }

        private Response<AccessTokenDTO> GenerateLoginTokens(AuthenticationUser user)
        {
            var decryptedUser = _userMethodsService.DecryptUserData(user);
            var userDto = _mapper.Map<UserDTO>(decryptedUser.Data);

            string accessToken = _jwtService.GenerateAccessToken(userDto);
            string refreshToken = user.Id.ToString();

            // Background jobs for cache and token management
            BackgroundJob.Enqueue(() => _redisService.SetTokenLogin(userDto, accessToken, TimeSpan.FromMinutes(15)));
            BackgroundJob.Enqueue(() => _redisService.SetRefreshToken(refreshToken));

            var result = new AccessTokenDTO
            {
                AccessToken = accessToken,
                IsEmailConfirmed = user.IsEmailConfirmed,
                IsStatusConfirmed = user.IsStatusConfirmed
            };

            var refreshCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(8),
                Path = "/"
            };

            _httpContextAccessor.HttpContext!.Response.Cookies.Append(
                "RefreshToken",
                refreshToken,
                refreshCookieOptions);

            return Response<AccessTokenDTO>.Success(result, "Login successful");
        }

        private async Task<Response<string>> CheckForDuplicateUser(string email, string phone, string companyName)
        {
            var existingEmail = await _userMethodsService.GetUserByEmail(email, true);
            if (existingEmail.Data != null)
                return Response<string>.Failure($"Email: {email} is already in use.");

            var existingPhone = await _userMethodsService.GetUserByPhoneNumber(phone, true);
            if (existingPhone.Data != null)
                return Response<string>.Failure($"PhoneNumber: {phone} is already in use.");

            var existingCompany = await _userMethodsService.GetUserByCompanyName(companyName, true);
            if (existingCompany.Data != null)
                return Response<string>.Failure($"CompanyName: {companyName} is already in use.");

            return Response<string>.Success("No duplicates found");
        }

        private Response<AuthenticationUser> CreateNewUser(RegisterUserDTO dto)
        {

            var newUser = _mapper.Map<AuthenticationUser>(dto);
            newUser.Email = _helperMethodService.EncryptStringSafe(_helperMethodService.Normalize(dto.Email!));
            newUser.PhoneNumber = _helperMethodService.EncryptStringSafe(_helperMethodService.Normalize(dto.PhoneNumber!));
            newUser.CompanyName = _helperMethodService.EncryptStringSafe(_helperMethodService.Normalize(dto.CompanyName!));
            newUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.PasswordHash);
            newUser.CreatedAt = DateTime.UtcNow;
            return Response<AuthenticationUser>.Success(newUser, $"Create New User successful.");
        }
    }
}
