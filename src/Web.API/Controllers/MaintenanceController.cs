using Microsoft.AspNetCore.Mvc;
using Application.Services.Interfaces;
using Web.API.Shared;
using Application.DTOs.Maintenance;
using Microsoft.AspNetCore.Authorization;

namespace Web.API.Controllers
{
    [ApiController]
    [Route("maintenance")]
    public class MaintenanceController : BaseApiController
    {
        private readonly IMaintenanceService maintenanceService;

        public MaintenanceController(IMaintenanceService maintenanceService)
        {
            this.maintenanceService = maintenanceService;
        }
        #region GET


        [HttpGet("device/{deviceId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<MaintenanceRecordDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMaintenancesByDeviceAsync(int deviceId)
        {
            try
            {
                var maintenances = await maintenanceService.GetDeviceMaintenanceHistoryAsync(deviceId);
                return Ok(maintenances);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("technician/{technicianId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<MaintenanceRecordDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMaintenancesByTechnicianAsync(int technicianId)
        {
            try
            {
                var maintenances = await maintenanceService.GetTechnicianMaintenanceHistoryAsync(technicianId);
                return Ok(maintenances);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("maintenance/{maintenanceId}")]
        [ProducesResponseType(typeof(ApiResponse<MaintenanceRecordDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMaintenanceRecordAsync(int maintenanceId)
        {
            try
            {
                var maintenance = await maintenanceService.GetMaintenanceRecordAsync(maintenanceId);
                return Ok(maintenance);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("maintenances")]
        [Authorize(Roles = "Director")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<MaintenanceRecordDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllMaintenanceRecordsAsync()
        {
            try
            {
                var maintenances = await maintenanceService.GetAllMaintenanceHistoryAsync();
                return Ok(maintenances);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion


        #region POST
        [HttpPost]
        [Authorize(Roles = "Technician")]
        [ProducesResponseType(typeof(ApiResponse<MaintenanceRecordDto>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateMaintenanceRecordAsync([FromBody] MaintenanceRecordDto createDto)
        {
            try
            {
                await maintenanceService.RegisterMaintenanceAsync(createDto);
                return Ok("Registered");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion 
        // Additional endpoints can be added here following the same pattern
    }
}