using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Services.Interfaces;
using Application.DTOs.Personnel;
using Application.DTOs.Inventory;
using Web.API.Shared;

namespace Web.API.Controllers
{
    [ApiController]
    [Route("personnel")]
    [Authorize]
    [Produces("application/json")]
    public class PersonnelController : BaseApiController
    {
        private readonly IPersonnelService _personnelService;
        private readonly ILogger<PersonnelController> _logger;

        public PersonnelController(IPersonnelService personnelService, ILogger<PersonnelController> logger)
        {
            _personnelService = personnelService;
            _logger = logger;
        }
        #region GET
        [HttpGet("technicians")]
        [Authorize(Roles = "Administrator,Director")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<TechnicianDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCompanyTechniciansAsync()
        {
            try
            {
                var technicians = await _personnelService.GetAllTechniciansAsync();
                return Ok(technicians);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving company technicians");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("technician/{technicianId}")]
        [ProducesResponseType(typeof(ApiResponse<TechnicianDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTechnicianByIdAsync(int technicianId)
        {
            try
            {
                var technicianDetails = await _personnelService.GetTechnicianAsync(technicianId);
                return Ok(technicianDetails);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving technician with ID {technicianId}");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("bonuses/{technicianId:int}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<BonusDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTechnicianBonusesAsync(int technicianId)
        {
            try
            {
                var bonuses = await _personnelService.GetTechnicianBonusesAsync(technicianId);
                return Ok(bonuses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving company bonuses");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("penalties/{technicianId:int}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<PenaltyDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTechnicianPenaltiesAsync(int technicianId)
        {
            try
            {
                var penalties = await _personnelService.GetTechnicianPenaltyAsync(technicianId);
                return Ok(penalties);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving company penalties");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("technician/{technicianId}/detail")]
        [Authorize(Roles = "Administrator,Director,SectionManager")]
        [ProducesResponseType(typeof(ApiResponse<TechnicianDetailDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTechnicianDetailByIdAsync(int technicianId)
        {
            try
            {
                var technicianDetails = await _personnelService.GetTechnicianDetailAsync(technicianId);
                return Ok(ApiResponse<TechnicianDetailDto>.Ok(technicianDetails));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving technician details with ID {technicianId}");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("performances/{technicianId:int}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<RateDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTechnicianPerformancesAsync(int technicianId)
        {
            try
            {
                var performances = await _personnelService.GetTechnicianPerformanceHistoryAsync(technicianId);
                return Ok(performances);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving company performances");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("pending-devices/{technicianId:int}")]
        [Authorize(Roles = "Technician")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<DeviceDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTechnicianPendingDevicesAsync([FromBody] int technicianId)
        {
            try
            {
                var pendingDevices = await _personnelService.GetTechnicianPendingDevicesAsync(technicianId);
                return Ok(pendingDevices);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving pending devices for technician {TechnicianId}", technicianId);
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region PUT

        [HttpPut("technician/{technicianId:int}")]
        [Authorize(Roles = "Administrator,Director")]
        [ProducesResponseType(typeof(ApiResponse<TechnicianDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateTechnicianAsync(int technicianId, [FromBody] UpdateTechnicianRequest request)
        {
            try
            {
                request.TechnicianId = technicianId;
                var technician = await _personnelService.UpdateTechnicianAsync(request);
                _logger.LogInformation("Técnico {TechnicianId} actualizado exitosamente", technicianId);
                return Ok(technician);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating technician {TechnicianId}", technicianId);
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region POST

        [HttpPost("rate")]
        [Authorize(Roles = "Administrator,Director,SectionManager")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> RateTechnicianAsync([FromBody] RateTechnicianRequest request)
        {
            try
            {
                await _personnelService.RateTechnicianPerformanceAsync(request);
                return Ok(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rating technician");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("bonus")]
        [Authorize(Roles = "Administrator,Director")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddTechnicianBonusAsync([FromBody] BonusRequest request)
        {
            try
            {
                await _personnelService.RegisterBonusAsync(request);
                return Ok(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding technician bonus");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("penalty")]
        [Authorize(Roles = "Administrator,Director")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddTechnicianPenaltyAsync([FromBody] PenaltyRequest request)
        {
            try
            {
                await _personnelService.RegisterPenaltyAsync(request);
                return Ok(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding technician penalty");
                return BadRequest(ex.Message);
            }
        }
        #endregion
    }

}