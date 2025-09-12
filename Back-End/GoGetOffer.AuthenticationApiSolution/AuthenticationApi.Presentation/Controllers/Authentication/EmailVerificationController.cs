using AuthenticationApi.Application.Services.Interfaces.AuthenticationService.EmailService;
using GoGetOffer.SharedLibrarySolution.DTOs.Auth.User.ConfirmEmail;
using GoGetOffer.SharedLibrarySolution.Responses;
using GoGetOffer.SharedLibrarySolution.Service.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationApi.Presentation.Controllers.Authentication
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User,Supplier,Client")]
    public class EmailVerificationController(IUserEmailService userEmail) : ControllerBase
    {
        [HttpPost("confirm")]
        public async Task<ActionResult<Response<ConfirmEmailOtpDTO>>> ConfirmEmail([FromBody] ConfirmEmailOtpDTO dto)
        {
            var validationResult = UserHelper.TryGetUserId<ConfirmEmailOtpDTO>(User, out Guid userId);
            if (validationResult != null)
                return BadRequest(validationResult);

            var result = await userEmail.ConfirmEmailOtpAsync(userId, dto.Otp!);
            return result.Status ? Ok(result) : BadRequest(result);
        }

        [HttpPost("send")]
        public async Task<ActionResult<Response<ConfirmEmailOtpDTO>>> SendConfirmEmail()
        {
            var validationResult = UserHelper.TryGetUserId<ConfirmEmailOtpDTO>(User, out Guid userId);
            if (validationResult != null)
                return BadRequest(validationResult);

            var result = await userEmail.SendEmailOtpAsync(userId);
            return result.Status ? Ok(result) : BadRequest(result);
        }
    }
}
