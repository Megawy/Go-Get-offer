using AuthenticationApi.Application.Services.Interfaces.AuthenticationService.ActionService;
using AuthenticationApi.Application.Services.Interfaces.AuthenticationService.QueryService;
using AuthenticationApi.Application.Services.Interfaces.AuthenticationService.UserUpdateService;
using GoGetOffer.SharedLibrarySolution.DTOs;
using GoGetOffer.SharedLibrarySolution.DTOs.Auth.User.UpdateUser;
using GoGetOffer.SharedLibrarySolution.DTOs.Auth.User.UserProfile;
using GoGetOffer.SharedLibrarySolution.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationApi.Presentation.Controllers.Authentication
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Admin,Staff")]
    [Authorize(Roles = "Admin,Staff,User")]
    public class AdminController(IUserActionService userAdmin,
        IUserQueryService userQuery,
        IUserUpdateCommandService userUpdate,
        IUserUpdateQueryService userUpdateQuery) : ControllerBase
    {
        [HttpGet("users")]
        public async Task<ActionResult<Response<IEnumerable<UserDTO>>>> GetAllUser()
        {
            var result = await userQuery.GetAllUsersAsync(true);
            return result.Status ? Ok(result) : NotFound(result);
        }

        [HttpDelete("profile/delete")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Response<UserDTO>>> DeleteUser([FromBody] IDDTO dto)
        {
            var result = await userAdmin.SoftDeleteUserAsync(dto.Id);
            return result.Status ? Ok(result) : BadRequest(result);
        }

        [HttpPut("profile/undelete")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Response<UserDTO>>> UnDeleteUser([FromBody] IDDTO dto)
        {
            var result = await userAdmin.UnSoftDeleteUserAsync(dto.Id);
            return result.Status ? Ok(result) : BadRequest(result);
        }

        [HttpPut("profile/ban")]
        public async Task<ActionResult<Response<UserDTO>>> BanUser([FromBody] IDDTO dto)
        {
            var result = await userAdmin.BanUserAsync(dto.Id);
            return result.Status ? Ok(result) : BadRequest(result);
        }

        [HttpPut("profile/unban")]
        public async Task<ActionResult<Response<UserDTO>>> UnBanUser([FromBody] IDDTO dto)
        {
            var result = await userAdmin.UnbanUserAsync(dto.Id);
            return result.Status ? Ok(result) : BadRequest(result);
        }

        [HttpGet("profile/update")]
        public async Task<ActionResult<Response<IEnumerable<GetRequestUserUpdateDTO>>>> GetAllRequest()
        {
            var result = await userUpdateQuery.GetAllRequestPendingUpdateUser();
            return result.Status ? Ok(result) : NotFound(result);
        }

        [HttpGet("profile/update-id")]
        public async Task<ActionResult<Response<GetRequestUserUpdateDTO>>> GetRequestUpdateByid([FromBody] IDDTO dto)
        {
            var result = await userUpdateQuery.GetRequestUserUpdateById(dto.Id);
            return result.Status ? Ok(result) : NotFound(result);
        }

        [HttpPut("profile/update")]
        public async Task<ActionResult<Response<UserDTO>>> UpdateUserProfile([FromBody] ApproveUserUpdateDTO dto)
        {
            var result = await userUpdate.ApproveUserUpdateAsync(dto);
            return result.Status ? Ok(result) : BadRequest(result);
        }

        [HttpPut("profile/role-change")]
        public async Task<ActionResult<Response<UserDTO>>> ChangeRole([FromBody] ChangeRoleDTO dto)
        {
            var result = await userAdmin.ChangeRole(dto);
            return result.Status ? Ok(result) : BadRequest(result);
        }
    }
}
