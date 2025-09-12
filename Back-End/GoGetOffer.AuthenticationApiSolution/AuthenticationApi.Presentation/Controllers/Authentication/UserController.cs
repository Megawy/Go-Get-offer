using AuthenticationApi.Application.Services.Interfaces.AuthenticationService.QueryService;
using AuthenticationApi.Application.Services.Interfaces.AuthenticationService.UserUpdateService;
using GoGetOffer.SharedLibrarySolution.DTOs.Auth.User.UpdateUser;
using GoGetOffer.SharedLibrarySolution.DTOs.Auth.User.UserProfile;
using GoGetOffer.SharedLibrarySolution.Responses;
using GoGetOffer.SharedLibrarySolution.Service.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationApi.Presentation.Controllers.Authentication
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User,Supplier,Client")]
    public class UserController(IUserQueryService userQuery,
        IUserUpdateCommandService userUpdate,
        IUserUpdateQueryService userUpdateQuery) : ControllerBase
    {
        [HttpGet()]
        public async Task<ActionResult<Response<UserDTO>>> GetUser()
        {
            var validationResult = UserHelper.TryGetUserId<UserDTO>(User, out Guid userId);
            if (validationResult != null)
                return validationResult;

            var result = await userQuery.GetUserByIdAsync(userId);
            return result.Status ? Ok(result) : BadRequest(result);
        }

        [HttpGet("profile/update-request")]
        public async Task<ActionResult<Response<GetRequestUserUpdateDTO>>> GetRequestEditByUserId()
        {
            var validationResult = UserHelper.TryGetUserId<GetRequestUserUpdateDTO>(User, out Guid userId);
            if (validationResult != null)
                return validationResult;

            var result = await userUpdateQuery.GetRequestUserUpdateByUserId(userId);
            return result.Status ? Ok(result) : BadRequest(result);
        }

        [HttpPut("profile/update-request")]
        public async Task<ActionResult<Response<RequestUserUpdateDTO>>> EditUser([FromBody] RequestUserUpdateDTO dto)
        {
            var validationResult = UserHelper.TryGetUserId<RequestUserUpdateDTO>(User, out Guid userId);
            if (validationResult != null)
                return validationResult;

            dto.AuthenticationUserId = userId;
            var result = await userUpdate.RequestUserUpdateAsync(dto);
            return result.Status ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("profile/update-request")]
        public async Task<ActionResult<Response<GetRequestUserUpdateDTO>>> CancelRequestEdit()
        {
            var validationResult = UserHelper.TryGetUserId<GetRequestUserUpdateDTO>(User, out Guid userId);
            if (validationResult != null)
                return validationResult;

            var result = await userUpdate.CancelRequestUserUpdateAsync(userId);
            return result.Status ? Ok(result) : BadRequest(result);
        }
    }
}
