using AuthenticationApi.Application.Services.Interfaces.AuthenticationService;
using AuthenticationApi.Application.Services.Interfaces.AuthenticationService.UserUpdateService;
using AuthenticationApi.Infrastructure.Repositories.Interfaces.Authentication;
using AutoMapper;
using GoGetOffer.SharedLibrarySolution.DTOs.Auth.User.UpdateUser;
using GoGetOffer.SharedLibrarySolution.Responses;

namespace AuthenticationApi.Application.Services.AuthenticationService.UserUpdateService
{
    public class UserUpdateQueryService : IUserUpdateQueryService
    {
        private readonly IMapper _mapper;
        private readonly IUserMethodsService _helperMethods;
        private readonly IRequestUserUpdateRepository _userUpdate;
        public UserUpdateQueryService(IMapper mapper,
            IUserMethodsService userMethodsService,
            IRequestUserUpdateRepository userUpdate)
        {
            _mapper = mapper;
            _helperMethods = userMethodsService;
            _userUpdate = userUpdate;
        }

        public async Task<Response<IEnumerable<GetRequestUserUpdateDTO>>> GetAllRequestPendingUpdateUser()
        {
            var response = await _userUpdate.GetAllAsync();
            if (!response.Status || response.Data is null)
                return Response<IEnumerable<GetRequestUserUpdateDTO>>.Failure(response.Message!);

            var decryptedUsers = response.Data
                .Select(user => _helperMethods.DecryptRequestData(user))
                    .Where(r => r.Status && r.Data != null)
                        .Select(r => r.Data!)
                            .ToList();

            var dto = _mapper.Map<IEnumerable<GetRequestUserUpdateDTO>>(decryptedUsers);

            return Response<IEnumerable<GetRequestUserUpdateDTO>>.Success(dto, response.Message!);
        }

        public async Task<Response<GetRequestUserUpdateDTO>> GetRequestUserUpdateById(Guid Id)
        {
            var request = await _userUpdate.FindByIdAsync(Id);
            if (request.Data is null)
                return Response<GetRequestUserUpdateDTO>.Failure("Request not found");

            var decryptedRequest = _helperMethods.DecryptRequestData(request.Data);

            var dto = _mapper.Map<GetRequestUserUpdateDTO>(decryptedRequest);

            return Response<GetRequestUserUpdateDTO>.Success(dto, "Request retrieved successfully");
        }

        public async Task<Response<GetRequestUserUpdateDTO>> GetRequestUserUpdateByUserId(Guid Id)
        {
            var request = await _userUpdate.GetLastPendingRequestByUserIdAsync(Id);

            if (request.Data is null || !request.Status)
                return Response<GetRequestUserUpdateDTO>.Failure(request.Message ?? "Request not found");

            var decryptedRequest = _helperMethods.DecryptRequestData(request.Data);

            var dto = _mapper.Map<GetRequestUserUpdateDTO>(decryptedRequest.Data);

            return Response<GetRequestUserUpdateDTO>.Success(dto, "Request retrieved successfully");
        }
    }
}
