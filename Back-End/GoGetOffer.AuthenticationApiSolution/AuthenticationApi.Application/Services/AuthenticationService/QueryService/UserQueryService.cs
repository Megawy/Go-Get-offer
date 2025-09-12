using AuthenticationApi.Application.Services.Interfaces.AuthenticationService;
using AuthenticationApi.Application.Services.Interfaces.AuthenticationService.QueryService;
using AuthenticationApi.Infrastructure.Repositories.Interfaces.Authentication;
using AutoMapper;
using GoGetOffer.SharedLibrarySolution.DTOs.Auth.User.UserProfile;
using GoGetOffer.SharedLibrarySolution.Interface.Helper;
using GoGetOffer.SharedLibrarySolution.Responses;
using MessagePack;

namespace AuthenticationApi.Application.Services.AuthenticationService.QueryService
{
    public class UserQueryService
        (IUserRepository userRepository,
          IMapper mapper,
         IRedisQueryService redisService,
         IHelperMethodService helperMethodService,
         IUserMethodsService userMethodsService
        ) : IUserQueryService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IMapper _mapper = mapper;
        private readonly IRedisQueryService _redisService = redisService;
        private readonly IUserMethodsService _userMethodsService = userMethodsService;
        private readonly IHelperMethodService _helperMethodService = helperMethodService;


        public async Task<Response<UserDTO>> RefreshUserCache(Guid Id, bool includeDeleted)
        {
            var CheckAndCache = await _helperMethodService.CheckAndCache("RefreshUserCache", $"{Id}", 3, TimeSpan.FromMinutes(15));

            var userResult = await _userRepository.FindByIdAsync(Id, includeDeleted);
            if (!userResult.Status || userResult.Data is null)
                return Response<UserDTO>.Failure("User not found");


            var decryptedUser = _userMethodsService.DecryptUserData(userResult.Data);

            var userDto = _mapper.Map<UserDTO>(decryptedUser.Data);

            if (!CheckAndCache.Status)
                await _redisService.SetUserInfo(userDto);

            return Response<UserDTO>.Success(userDto, "User cache refreshed successfully.");
        }

        public async Task<Response<IEnumerable<UserDTO>>> RefreshUsersCache()
        {
            var CheckAndCache = await _helperMethodService.CheckAndCache("RefreshUsersCache", $"all", 3, TimeSpan.FromMinutes(15));

            var response = await _userRepository.GetAllAsync(true);
            if (!response.Status || response.Data is null)
                return Response<IEnumerable<UserDTO>>.Failure(response.Message ?? "Failed to get users");

            var decryptedUsers = response.Data
                .Select(_userMethodsService.DecryptUserData)
                    .Where(r => r.Status && r.Data != null)
                        .Select(r => r.Data!)
                            .ToList();

            var userDtos = _mapper.Map<IEnumerable<UserDTO>>(decryptedUsers);

            if (!CheckAndCache.Status)
                await _redisService.SetAllUser(userDtos);

            return Response<IEnumerable<UserDTO>>.Success(userDtos, "Users cache refreshed successfully.");
        }


        public async Task<Response<UserDTO>> GetUserByIdAsync(Guid Id, bool includeDeleted = false)
        {
            var userCache = await _redisService.GetUserInfo(Id);
            if (userCache.Status && userCache.Data is not null)
            {
                var userObj = MessagePackSerializer.Deserialize<UserDTO>(userCache.Data);
                return Response<UserDTO>.Success(userObj, "User retrieved from cache");
            }

            var user = await RefreshUserCache(Id, includeDeleted);
            return user.Status
                ? Response<UserDTO>.Success(user.Data, "User retrieved from database")
                : user;
        }

        public async Task<Response<IEnumerable<UserDTO>>> GetAllUsersAsync(bool includeDeleted = false)
        {
            var usersCache = await _redisService.GetAllUser();
            if (usersCache.Status && usersCache.Data != null && usersCache.Data.Length > 0)
            {
                var userObj = MessagePackSerializer.Deserialize<IEnumerable<UserDTO>>(usersCache.Data);
                return Response<IEnumerable<UserDTO>>.Success(userObj, "Users retrieved from cache");
            }

            var response = await RefreshUsersCache();
            return response.Status
                ? Response<IEnumerable<UserDTO>>.Success(response.Data, "Users retrieved from database")
                : response;
        }
    }
}
