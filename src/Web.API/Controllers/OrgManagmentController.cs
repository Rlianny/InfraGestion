using Application.DTOs.Auth;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.API.Shared;

namespace Web.API.Controllers
{
    [ApiController]
    [Route("organization")]
    [Authorize(Roles = "Administrator")]
    public class OrgManagmentController : BaseApiController
    {
        private readonly IOrgManagementService orgManagementService;
        public OrgManagmentController(IOrgManagementService orgManagementService)
        {
            this.orgManagementService = orgManagementService;
        }

        #region GET
        [HttpGet("departments")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<DepartmentDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDepartmentsAsync()
        {
            try
            {
                var departments = await orgManagementService.GetDepartmentsAsync();
                return Ok(departments);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("sections")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<SectionDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSectionsAsync()
        {
            try
            {
                var sections = await orgManagementService.GetSectionsAsync();
                return Ok(sections);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("sections/managers")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<UserDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSectionManagersAsync()
        {
            try
            {
                var managers = await orgManagementService.GetSectionManagersAsync();
                return Ok(managers);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region POST
        [HttpPost("sections")]
        [ProducesResponseType(typeof(ApiResponse<string?>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateSectionAsync([FromBody] SectionDto sectionDto)
        {
            try
            {
                await orgManagementService.CreateSection(sectionDto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("departments")]
        [ProducesResponseType(typeof(ApiResponse<string?>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateDepartmentAsync([FromBody] DepartmentDto departmentDto)
        {
            try
            {
                await orgManagementService.CreateDepartment(departmentDto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("sections/managers")]
        [ProducesResponseType(typeof(ApiResponse<string?>), StatusCodes.Status200OK)]
        public async Task<IActionResult> AssignSectionResponsableAsync([FromBody] AssignSectionResponsibleDto assignSectionResponsibleDto)
        {
            try
            {
                await orgManagementService.AssignSectionResponsible(assignSectionResponsibleDto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("sections/disables{id}")]
        [ProducesResponseType(typeof(ApiResponse<string?>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DisableSectionAsync(int id)
        {
            try
            {
                await orgManagementService.DisableSection(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("departments/disables{id}")]
        [ProducesResponseType(typeof(ApiResponse<string?>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DisableDepartmentAsync(int id)
        {
            try
            {
                await orgManagementService.DisableDepartment(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
        #region PUT
        [HttpPut("sections")]
        [ProducesResponseType(typeof(ApiResponse<string?>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ModifySectionAsync([FromBody] SectionDto sectionDto)
        {
            try
            {
                await orgManagementService.ModifySection(sectionDto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("departments")]
        [ProducesResponseType(typeof(ApiResponse<string?>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ModifyDepartmentAsync([FromBody] DepartmentDto departmentDto)
        {
            try
            {
                await orgManagementService.ModifyDepartment(departmentDto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion
        #region DELETE
        [HttpDelete("departments{id}")]
        [ProducesResponseType(typeof(ApiResponse<string?>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteDepartmentsAsync(int id)
        {
            try
            {
                await orgManagementService.DeleteDepartment(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("sections{id}")]
        [ProducesResponseType(typeof(ApiResponse<string?>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteSectionAsync(int id)
        {
            try
            {
                await orgManagementService.DeleteSection(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
    }
}
