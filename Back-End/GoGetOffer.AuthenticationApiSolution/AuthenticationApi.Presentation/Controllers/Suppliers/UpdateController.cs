using AuthenticationApi.Application.Services.Interfaces.SupplierService.ProfileUpdateService;
using GoGetOffer.SharedLibrarySolution.DTOs;
using GoGetOffer.SharedLibrarySolution.DTOs.Supplier.UpdateRequest;
using GoGetOffer.SharedLibrarySolution.Responses;
using GoGetOffer.SharedLibrarySolution.Service.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationApi.Presentation.Controllers.Suppliers
{
    [Route("api/Supplier/[controller]")]
    [ApiController]
    [Authorize(Roles = "Supplier")]
    public class UpdateController(ISupplierUpdateQueryService supplierUpdateQueryService, ISupplierUpdateCommandService supplierUpdateCommand) : ControllerBase
    {
        [HttpGet]
        [Authorize(Roles = "Supplier,Admin,Staff")]
        public async Task<ActionResult<Response<SupplierUpdateProfileDTO>>> GetRequestById([FromBody] IDDTO dto)
        {
            var result = await supplierUpdateQueryService.GetRequestById(dto.Id);
            return result.Status ? Ok(result) : NotFound(result);
        }

        [HttpGet("last")]
        public async Task<ActionResult<Response<SupplierUpdateProfileDTO>>> GetLastRequestByUserId()
        {
            var validationResult = UserHelper.TryGetUserId<SupplierUpdateProfileDTO>(User, out Guid userId);
            if (validationResult != null)
                return validationResult;

            var result = await supplierUpdateQueryService.GetRequestById(userId);
            return result.Status ? Ok(result) : NotFound(result);
        }

        [HttpPost]
        public async Task<ActionResult<Response<SupplierUpdateProfileDTO>>> MakeRequestUpdateSupplier([FromBody] CreateRequestSupplierUpdateProfileDTO dto)
        {
            var validationResult = UserHelper.TryGetUserId<SupplierUpdateProfileDTO>(User, out Guid userId);
            if (validationResult != null)
                return validationResult;
            dto.UserId = userId;
            var result = await supplierUpdateCommand.MakeRequestUserUpdate(dto);
            return result.Status ? Ok(result) : NotFound(result);
        }

        [HttpDelete]
        public async Task<ActionResult<Response<SupplierUpdateProfileDTO>>> DelLastRequestByUserId()
        {
            var validationResult = UserHelper.TryGetUserId<SupplierUpdateProfileDTO>(User, out Guid userId);
            if (validationResult != null)
                return validationResult;
            var result = await supplierUpdateCommand.CancelRequestUserUpdateByUserId(userId);
            return result.Status ? Ok(result) : NotFound(result);
        }
    }
}
