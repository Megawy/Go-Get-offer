using AuthenticationApi.Application.Services.Interfaces.AuthenticationService.PasswordService;
using GoGetOffer.SharedLibrarySolution.DTOs.Auth.User.Login;
using GoGetOffer.SharedLibrarySolution.DTOs.Auth.User.UserPassword;
using GoGetOffer.SharedLibrarySolution.Responses;
using GoGetOffer.SharedLibrarySolution.Service.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationApi.Presentation.Controllers.Authentication
{
    [Route("api/profile/[controller]")]
    [ApiController]
    public class PasswordController(IUserPasswordService userPassword) : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost("forget")]
        public async Task<ActionResult<Response<TokenDTO>>> ForgotPassword([FromBody] ForgotPasswordDTO dto)
        {
            var result = await userPassword.ForgotPasswordAsync(dto.Email!);
            return result.Status ? Ok(result) : BadRequest(result);
        }

        [AllowAnonymous]
        [HttpPost("reset")]
        public async Task<ActionResult<Response<AccessTokenDTO>>> ResetPassword([FromBody] ResetPasswordDTO dto)
        {
            var result = await userPassword.ResetPasswordAsync(dto);
            return result.Status ? Ok(result) : BadRequest(result);
        }

        [HttpPut("change")]
        [Authorize(Roles = "User,Supplier,Client")]
        public async Task<ActionResult<Response<AccessTokenDTO>>> ChangePassword([FromBody] ChangePasswordDTO dto)
        {
            var validationResult = UserHelper.TryGetUserId<AccessTokenDTO>(User, out Guid userId);
            if (validationResult != null)
                return validationResult;
            dto.Id = userId;
            var result = await userPassword.ChangePasswordAsync(dto);
            return result.Status ? Ok(result) : BadRequest(result);
        }
    }
}
