using AuthenticationApi.Application.Services.Interfaces.SupplierService.JoinService;
using AuthenticationApi.Application.Services.Interfaces.SupplierService.ProFileService;
using AuthenticationApi.Application.Services.Interfaces.SupplierService.ProfileUpdateService;
using GoGetOffer.SharedLibrarySolution.DTOs;
using GoGetOffer.SharedLibrarySolution.DTOs.Supplier.JoinRequest;
using GoGetOffer.SharedLibrarySolution.DTOs.Supplier.Profile;
using GoGetOffer.SharedLibrarySolution.DTOs.Supplier.UpdateRequest;
using GoGetOffer.SharedLibrarySolution.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationApi.Presentation.Controllers.Suppliers
{
    [Route("api/Supplier/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Staff")]
    public class AdminController
        (ISupplierQueryService supplierProfileJoinService,
        ISupplierJoinQueryService supplierJoinQueryService,
        ISupplierJoinCommandService supplierJoinCommand,
        ISupplierCommandService supplierProfile,
        ISupplierUpdateQueryService supplierUpdateQueryService,
        ISupplierUpdateCommandService updateCommandService) : ControllerBase
    {
        [HttpGet("join")]
        public async Task<ActionResult<Response<IEnumerable<ProfileJoinRequestDTO>>>> GetAllRequestPending()
        {
            var result = await supplierJoinQueryService.GetAllRequestPending();
            return result.Status ? Ok(result) : NotFound(result);
        }

        [HttpGet("join/id")]
        public async Task<ActionResult<Response<ProfileJoinRequestDTO>>> GetRequestById([FromBody] IDDTO dto)
        {
            var result = await supplierJoinQueryService.GetRequestById(dto.Id);
            return result.Status ? Ok(result) : NotFound(result);
        }

        [HttpPost("join/id")]
        public async Task<ActionResult<Response<ProfileJoinRequestDTO>>> ReplyRequestJoin(ReplyRequestJoinDTO dto)
        {
            var result = await supplierJoinCommand.ReplyRequestJoin(dto);
            return result.Status ? Ok(result) : NotFound(result);
        }

        [HttpGet("Supplieres")]
        public async Task<ActionResult<Response<IEnumerable<SupplierProfileDTO>>>> GetProfiles()
        {
            var result = await supplierProfileJoinService.GetProfiles();
            return result.Status ? Ok(result) : NotFound(result);
        }

        [HttpGet("Profile")]
        public async Task<ActionResult<Response<SupplierProfileDTO>>> GetProfileById([FromBody] IDDTO dto)
        {
            var result = await supplierProfileJoinService.GetProfileById(dto.Id);
            return result.Status ? Ok(result) : NotFound(result);
        }

        [HttpGet("Pending")]
        public async Task<ActionResult<Response<SupplierProfileDTO>>> GetAllProfileRequest()
        {
            var result = await supplierUpdateQueryService.GetAllRequestPending();
            return result.Status ? Ok(result) : NotFound(result);
        }

        [HttpDelete("Profile")]
        public async Task<ActionResult<Response<SupplierProfileDTO>>> DeleteProfile([FromBody] IDDTO dto)
        {
            var result = await supplierProfile.DeleteProfile(dto.Id);
            return result.Status ? Ok(result) : BadRequest(result);
        }

        [HttpPut("update")]
        public async Task<ActionResult<Response<SupplierProfileDTO>>> ApproveSupplierUpdate([FromBody] ApproveSupplierUpdateDTO dto)
        {
            var result = await updateCommandService.ApproveSupplierUpdate(dto);
            return result.Status ? Ok(result) : BadRequest(result);
        }
    }
}
