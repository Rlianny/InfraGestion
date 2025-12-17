using Application.DTOs.Report;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Web.API.Shared;

namespace Web.API.Controllers
{
    [ApiController]
    [Route("reports")]

    public class ReportController : BaseApiController
    {
        private readonly IReportService reportService;

        public ReportController(IReportService reportService)
        {
            this.reportService = reportService;
        }

        #region GET
        

        [HttpGet("decommissionings")]
        [ProducesResponseType(typeof(ApiResponse<Report<DecommissioningReportDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GenerateDecommissioningReportAsync()
        {
            try
            {
                var report = await reportService.GenerateDischargeReportAsync();
                return Ok(report);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("personnel-effectiveness")]
        [ProducesResponseType(typeof(ApiResponse<Report<PersonnelEffectivenessReportDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GeneratePersonnelEffectivenessReportAsync([FromQuery] PersonnelReportFilterDto criteria)
        {
            try
            {
                var report = await reportService.GeneratePersonnelEffectivenessReportAsync(criteria);
                return Ok(report);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("equipment-maintenances/{deviceId}")]
        [ProducesResponseType(typeof(ApiResponse<Report<Report<DeviceMantainenceReportDto>>>),StatusCodes.Status200OK)]
        public async Task<IActionResult> GenerateDeviceMantinenceReport(int deviceId)
        {
            try
            {
                var report =await reportService.GenerateDeviceMantainanceReportAsync(deviceId);
                return Ok(report);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);  
            }
        }
        [HttpGet("equipment-replacement")]
        [ProducesResponseType(typeof(ApiResponse<Report<DeviceReplacementReportDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GenerateEquipmentReplacementReportAsync()
        {
            try
            {
                var report = await reportService.GenerateEquipmentReplacementReportAsync();
                return Ok(report);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("transfers")]
        [ProducesResponseType(typeof(ApiResponse<Report<SectionTransferReportDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GenerateTransferReportAsync()
        {
            try
            {
                var report = await reportService.GenerateTransferReportAsync();
                return Ok(report);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("correlation-analysis")]
        [ProducesResponseType(typeof(ApiResponse<Report<CorrelationAnalysisReportDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GenerateCorrelationAnalysisReportAsync()
        {
            try
            {
                var report = await reportService.GenerateCorrelationAnalysisReportAsync();
                return Ok(report);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("bonus-determination")]
        [ProducesResponseType(typeof(ApiResponse<Report<BonusDeterminationReportDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GenerateBonusDeterminationReportAsync()
        {
            try
            {
                var report = await reportService.GenerateBonusDeterminationReportAsync();
                return Ok(report);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
   
        #endregion
    }
}
