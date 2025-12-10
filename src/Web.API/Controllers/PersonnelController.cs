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
    // [Authorize]
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
        // [Authorize(Roles = "Administrator,Director")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<TechnicianDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCompanyTechniciansAsync()
        {
            try
            {
                var technicians = await _personnelService.GetAllTechniciansAsync();
                return Ok(ApiResponse<IEnumerable<TechnicianDto>>.Ok(technicians));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving company technicians");
                return BadRequest(ApiResponse<IEnumerable<TechnicianDto>>.Fail(ex.Message));
            }
        }

        [HttpGet("technician/{technicianId:int}")]
        [ProducesResponseType(typeof(ApiResponse<TechnicianDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTechnicianByIdAsync(int technicianId)
        {
            try
            {
                var technicianDetails = await _personnelService.GetTechnicianAsync(technicianId);
                return Ok(ApiResponse<TechnicianDto>.Ok(technicianDetails));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving technician with ID {TechnicianId}", technicianId);
                return BadRequest(ApiResponse<TechnicianDto>.Fail(ex.Message));
            }
        }

        [HttpGet("bonuses/{technicianId:int}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<BonusDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTechnicianBonusesAsync(int technicianId)
        {
            try
            {
                var bonuses = await _personnelService.GetTechnicianBonusesAsync(technicianId);
                return Ok(ApiResponse<IEnumerable<BonusDto>>.Ok(bonuses));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving bonuses for technician {TechnicianId}", technicianId);
                return BadRequest(ApiResponse<IEnumerable<BonusDto>>.Fail(ex.Message));
            }
        }

        [HttpGet("penalties/{technicianId:int}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<PenaltyDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTechnicianPenaltiesAsync(int technicianId)
        {
            try
            {
                var penalties = await _personnelService.GetTechnicianPenaltyAsync(technicianId);
                return Ok(ApiResponse<IEnumerable<PenaltyDto>>.Ok(penalties));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving penalties for technician {TechnicianId}", technicianId);
                return BadRequest(ApiResponse<IEnumerable<PenaltyDto>>.Fail(ex.Message));
            }
        }

        [HttpGet("technician/{technicianId:int}/detail")]
        // [Authorize(Roles = "Administrator,Director,SectionManager")]
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
                _logger.LogError(ex, "Error retrieving technician details with ID {TechnicianId}", technicianId);
                return BadRequest(ApiResponse<TechnicianDetailDto>.Fail(ex.Message));
            }
        }

        [HttpGet("performances/{technicianId:int}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<RateDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTechnicianPerformancesAsync(int technicianId)
        {
            try
            {
                var performances = await _personnelService.GetTechnicianPerformanceHistoryAsync(technicianId);
                return Ok(ApiResponse<IEnumerable<RateDto>>.Ok(performances));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving performances for technician {TechnicianId}", technicianId);
                return BadRequest(ApiResponse<IEnumerable<RateDto>>.Fail(ex.Message));
            }
        }

        [HttpGet("pending-devices/{technicianId:int}")]
        [Authorize(Roles = "Technician")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<DeviceDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTechnicianPendingDevicesAsync(int technicianId)
        {
            try
            {
                var pendingDevices = await _personnelService.GetTechnicianPendingDevicesAsync(technicianId);
                return Ok(ApiResponse<IEnumerable<DeviceDto>>.Ok(pendingDevices));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving pending devices for technician {TechnicianId}", technicianId);
                return BadRequest(ApiResponse<IEnumerable<DeviceDto>>.Fail(ex.Message));
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
                return Ok(ApiResponse<TechnicianDto>.Ok(technician));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating technician {TechnicianId}", technicianId);
                return BadRequest(ApiResponse<TechnicianDto>.Fail(ex.Message));
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
                return Ok(ApiResponse<bool>.Ok(true));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rating technician {TechnicianId}", request.TechnicianId);
                return BadRequest(ApiResponse<bool>.Fail(ex.Message));
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
                return Ok(ApiResponse<bool>.Ok(true));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding bonus for technician {TechnicianId}", request.TechnicianId);
                return BadRequest(ApiResponse<bool>.Fail(ex.Message));
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
                return Ok(ApiResponse<bool>.Ok(true));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding penalty for technician {TechnicianId}", request.TechnicianId);
                return BadRequest(ApiResponse<bool>.Fail(ex.Message));
            }
        }
        #endregion
    }

}