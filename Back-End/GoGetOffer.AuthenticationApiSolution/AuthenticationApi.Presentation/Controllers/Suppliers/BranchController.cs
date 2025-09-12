using AuthenticationApi.Application.Services.Interfaces.SupplierService.BranceService;
using GoGetOffer.SharedLibrarySolution.DTOs;
using GoGetOffer.SharedLibrarySolution.DTOs.Supplier.Branch;
using GoGetOffer.SharedLibrarySolution.DTOs.Supplier.Profile;
using GoGetOffer.SharedLibrarySolution.Responses;
using GoGetOffer.SharedLibrarySolution.Service.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationApi.Presentation.Controllers.Suppliers
{
    [Route("api/Supplier/[controller]")]
    [ApiController]
    [Authorize(Roles = "Supplier")]
    public class BranchController(ISupplierBranceQueryService supplierBranceQuery, ISupplierBranchCommandService branchCommandService) : ControllerBase
    {
        [HttpGet("all")]
        public async Task<ActionResult<Response<IEnumerable<SupplierBranchDTO>>>> GetAllBranch()
        {
            var validationResult = UserHelper.TryGetUserIdForEnumerable<SupplierBranchDTO>(User, out Guid userId);
            if (validationResult != null)
                return validationResult;
            var result = await supplierBranceQuery.GetAllBranchByUserId(userId);
            return result.Status ? Ok(result) : NotFound(result);
        }

        [HttpGet("code")]
        public async Task<ActionResult<Response<IEnumerable<SupplierBranchDTO>>>> GetAllBranchByCode([FromBody] CodeSupplierDTO dto)
        {
            var result = await supplierBranceQuery.GetBranchByCode(dto);
            return result.Status ? Ok(result) : NotFound(result);
        }

        [HttpGet()]
        public async Task<ActionResult<Response<SupplierBranchDTO>>> GetBranchById([FromBody] IDDTO dto)
        {
            var result = await supplierBranceQuery.GetBranchById(dto.Id);
            return result.Status ? Ok(result) : NotFound(result);
        }

        [HttpPost("Create")]
        public async Task<ActionResult<Response<SupplierBranchDTO>>> CreateBranch([FromBody] CreateBranchDTO dto)
        {

            var validationResult = UserHelper.TryGetUserId<SupplierBranchDTO>(User, out Guid userId);
            if (validationResult != null)
                return validationResult;
            dto.SupplierProfilesId = userId;
            var result = await branchCommandService.CreateBranch(dto);
            return result.Status ? Ok(result) : NotFound(result);
        }

        [HttpPut("Edit")]
        public async Task<ActionResult<Response<SupplierBranchDTO>>> UpdateBranch([FromBody] UpdateBranchDTO dto)
        {
            var result = await branchCommandService.UpdateBranch(dto);
            return result.Status ? Ok(result) : NotFound(result);
        }

        [HttpDelete("Delete")]
        public async Task<ActionResult<Response<SupplierBranchDTO>>> DeleteBranch([FromBody] IDDTO dto)
        {
            var result = await branchCommandService.DeleteBranch(dto.Id);
            return result.Status ? Ok(result) : NotFound(result);
        }
    }
}
