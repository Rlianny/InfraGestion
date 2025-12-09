using Application.DTOs.Auth;
using Application.Services.Interfaces;
using Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.API.Shared;

namespace Web.API.Controllers
{
    [ApiController]
    [Route("organization")]
    // [Authorize(Roles = "Administrator")]
    public class OrganizationController : BaseApiController
    {
        private readonly IOrgManagementService _orgManagementService;

        public OrganizationController(IOrgManagementService orgManagementService)
        {
            _orgManagementService = orgManagementService;
        }

        #region GET
        [HttpGet("departments")]
        [ProducesResponseType(
            typeof(ApiResponse<IEnumerable<DepartmentDto>>),
            StatusCodes.Status200OK
        )]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDepartmentsAsync()
        {
            try
            {
                var departments = await _orgManagementService.GetDepartmentsAsync();
                return Ok(departments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving departments", details = ex.Message });
            }
        }

        [HttpGet("sections")]
        [ProducesResponseType(
            typeof(ApiResponse<IEnumerable<SectionDto>>),
            StatusCodes.Status200OK
        )]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSectionsAsync()
        {
            try
            {
                var sections = await _orgManagementService.GetSectionsAsync();
                return Ok(sections);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving sections", details = ex.Message });
            }
        }

        [HttpGet("sections/managers")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<UserDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSectionManagersAsync()
        {
            try
            {
                var managers = await _orgManagementService.GetSectionManagersAsync();
                return Ok(managers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving section managers", details = ex.Message });
            }
        }
        #endregion

        #region POST
        [HttpPost("sections")]
        [ProducesResponseType(typeof(ApiResponse<string?>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateSectionAsync([FromBody] SectionDto sectionDto)
        {
            try
            {
                await _orgManagementService.CreateSection(sectionDto);
                return Ok();
            }
            catch (DuplicateEntityException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("departments")]
        [ProducesResponseType(typeof(ApiResponse<string?>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateDepartmentAsync(
            [FromBody] DepartmentDto departmentDto
        )
        {
            try
            {
                await _orgManagementService.CreateDepartment(departmentDto);
                return Ok();
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (DuplicateEntityException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [HttpPost("sections/disable/{id}")]
        [ProducesResponseType(typeof(ApiResponse<string?>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DisableSectionAsync(int id)
        {
            try
            {
                await _orgManagementService.DisableSection(id);
                return Ok();
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("departments/disable/{id}")]
        [ProducesResponseType(typeof(ApiResponse<string?>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DisableDepartmentAsync(int id)
        {
            try
            {
                await _orgManagementService.DisableDepartment(id);
                return Ok();
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        #endregion
        #region PUT
        [HttpPut("sections")]
        [ProducesResponseType(typeof(ApiResponse<string?>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> ModifySectionAsync([FromBody] SectionDto sectionDto)
        {
            try
            {
                await _orgManagementService.ModifySection(sectionDto);
                return Ok();
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (DuplicateEntityException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (BusinessRuleViolationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("departments")]
        [ProducesResponseType(typeof(ApiResponse<string?>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> ModifyDepartmentAsync(
            [FromBody] DepartmentDto departmentDto
        )
        {
            try
            {
                await _orgManagementService.ModifyDepartment(departmentDto);
                return Ok();
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (DuplicateEntityException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        #endregion
        #region DELETE
        [HttpDelete("departments/{id}")]
        [ProducesResponseType(typeof(ApiResponse<string?>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteDepartmentsAsync(int id)
        {
            try
            {
                await _orgManagementService.DeleteDepartment(id);
                return Ok();
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("sections/{id}")]
        [ProducesResponseType(typeof(ApiResponse<string?>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteSectionAsync(int id)
        {
            try
            {
                await _orgManagementService.DeleteSection(id);
                return Ok();
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        #endregion
    }
}
