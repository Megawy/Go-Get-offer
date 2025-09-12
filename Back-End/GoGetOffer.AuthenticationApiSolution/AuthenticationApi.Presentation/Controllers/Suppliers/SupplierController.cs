using AuthenticationApi.Application.Services.Interfaces.SupplierService.JoinService;
using AuthenticationApi.Application.Services.Interfaces.SupplierService.ProFileService;
using GoGetOffer.SharedLibrarySolution.DTOs.Supplier.Profile;
using GoGetOffer.SharedLibrarySolution.Responses;
using GoGetOffer.SharedLibrarySolution.Service.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationApi.Presentation.Controllers.Suppliers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController(ISupplierCommandService supplierProfile, ISupplierQueryService supplierQuery, ISupplierJoinQueryService supplierJoinQuery) : ControllerBase
    {
        [HttpPost("register")]
        [Consumes("multipart/form-data")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<Response<RegisterSupplierDTO>>> Register([FromForm] RegisterSupplierDTO dto)
        {
            var validationResult = UserHelper.TryGetUserId<RegisterSupplierDTO>(User, out Guid userId);
            if (validationResult != null)
                return validationResult;
            dto.Id = userId;
            var result = await supplierProfile.RegisterProfile(dto);
            return result.Status ? Created(string.Empty, result) : BadRequest(result);
        }

        [HttpPut("register")]
        [Consumes("multipart/form-data")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<Response<UpdateSupplierDTO>>> UpdateDataSupplier([FromForm] UpdateSupplierDTO dto)
        {
            var validationResult = UserHelper.TryGetUserId<UpdateSupplierDTO>(User, out Guid userId);
            if (validationResult != null)
                return validationResult;
            dto.Id = userId;
            var result = await supplierProfile.UpdateDataSupplier(dto);
            return result.Status ? Created(string.Empty, result) : BadRequest(result);
        }

        [HttpGet]
        [Authorize(Roles = "Supplier")]
        public async Task<ActionResult<Response<SupplierProfileDTO>>> GetProfileByUserId()
        {
            var validationResult = UserHelper.TryGetUserId<SupplierProfileDTO>(User, out Guid userId);
            if (validationResult != null)
                return validationResult;

            var result = await
                supplierQuery.GetProfileById(userId);
            return result.Status ? Ok(result) : NotFound(result);
        }

        [HttpGet("code")]
        [Authorize(Roles = "User,Supplier,Admin,Staff")]
        public async Task<ActionResult<Response<SupplierProfileDTO>>> GetProfileByCoded([FromBody] CodeSupplierDTO dto)
        {
            var result = await supplierQuery.GetProfileByCode(dto);
            return result.Status ? Ok(result) : NotFound(result);
        }
        [HttpGet("state")]
        public async Task<ActionResult<Response<SupplierProfileDTO>>> GetRequestProfileByUserId()
        {
            var validationResult = UserHelper.TryGetUserId<SupplierProfileDTO>(User, out Guid userId);
            if (validationResult != null)
                return validationResult;
            var result = await supplierJoinQuery.GetRequestById(userId);
            return result.Status ? Ok(result) : NotFound(result);
        }

        [HttpPut("data")]
        [Authorize(Roles = "Supplier")]
        public async Task<ActionResult<Response<AddSomeDataSupplierDTO>>> UpdateSomeData([FromBody] AddSomeDataSupplierDTO dto)
        {
            var validationResult = UserHelper.TryGetUserId<AddSomeDataSupplierDTO>(User, out Guid userId);
            if (validationResult != null)
                return validationResult;
            dto.Id = userId;
            var result = await supplierProfile.AddSomeDataSupplier(dto);
            return result.Status ? Ok(result) : BadRequest(result);
        }
    }
}
