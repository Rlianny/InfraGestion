using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Services.Interfaces;
using Application.DTOs.Personnel;
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
        [HttpGet("company/technicians")]
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

        [HttpGet("{technician}")]
        [ProducesResponseType(typeof(ApiResponse<TechnicianDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTechnicianByIdAsync(int technician)
        {
            try
            {
                var technicianDetails = await _personnelService.GetTechnicianAsync(technician);
                return Ok(technicianDetails);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving technician with ID {technician}");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("company/bonuses")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<BonusDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTechnicianBonusesAsync([FromQuery] int technicianId)
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

        [HttpGet("company/penalties")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<PenaltyDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTechnicianPenaltiesAsync([FromQuery] int technicianId)
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

        [HttpGet("company/performances")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<RateDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTechnicianPerformancesAsync([FromQuery] int technicianId)
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