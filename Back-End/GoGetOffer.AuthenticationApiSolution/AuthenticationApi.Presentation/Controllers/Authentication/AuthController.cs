using AuthenticationApi.Application.Services.Interfaces.AuthenticationService.AuthService;
using GoGetOffer.SharedLibrarySolution.DTOs.Auth.User.Login;
using GoGetOffer.SharedLibrarySolution.DTOs.Auth.User.Register;
using GoGetOffer.SharedLibrarySolution.Responses;
using GoGetOffer.SharedLibrarySolution.Service.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationApi.Presentation.Controllers.Authentication
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User,Supplier,Client")]
    public class AuthController
        (IUserAuthService userAuth) : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<ActionResult<Response<string>>> RefreshToken()
        {
            var refreshToken = Request.Cookies["RefreshToken"];

            if (!Guid.TryParse(refreshToken, out Guid userId))
            {
                var errorResponse = Response<string>.Failure("Invalid RefreshToken");
                return BadRequest(errorResponse);
            }
            var result = await userAuth.RefreshToken(userId);
            return result.Status ? Ok(result) : BadRequest(result);
        }

        [HttpPost("logout")]
        public async Task<ActionResult<Response<string>>> Logout()
        {
            var validationResult = UserHelper.TryGetUserId<string>(User, out Guid userId);
            if (validationResult != null)
                return BadRequest(validationResult);

            var result = await userAuth.Logout(userId);
            return result.Status ? Ok(result) : BadRequest(result);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<Response<string>>> Register([FromBody] RegisterUserDTO dto)
        {
            var result = await userAuth.RegisterAsync(dto);
            return result.Status ? Created(string.Empty, result) : BadRequest(result);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<Response<string>>> Login([FromBody] LoginDTO dto)
        {
            var result = await userAuth.LoginAsync(dto);
            return result.Status ? Ok(result) : BadRequest(result);
        }
    }
}
