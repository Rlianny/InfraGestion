using Application.DTOs.Report;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
        [HttpGet("inventory")]
        [ProducesResponseType(typeof(ApiResponse<DeviceReportDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GenerateInventoryReportAsync([FromQuery] DeviceReportFilterDto filter)
        {
            try
            {
                var report = await reportService.GenerateInventoryReportAsync(filter);
                return Ok(report);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("decommissionings")]
        [ProducesResponseType(typeof(ApiResponse<DecommissioningReportDto>), StatusCodes.Status200OK)]
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
        [ProducesResponseType(typeof(ApiResponse<PersonnelEffectivenessReportDto>), StatusCodes.Status200OK)]
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

        [HttpGet("equipment-replacement")]
        [ProducesResponseType(typeof(ApiResponse<DeviceReplacementReportDto>), StatusCodes.Status200OK)]
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
        [ProducesResponseType(typeof(ApiResponse<SectionTransferReportDto>), StatusCodes.Status200OK)]
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
        [ProducesResponseType(typeof(ApiResponse<CorrelationAnalysisReportDto>), StatusCodes.Status200OK)]
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
        [ProducesResponseType(typeof(ApiResponse<BonusDeterminationReportDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GenerateBonusDeterminationReportAsync([FromQuery] BonusReportCriteria criteria)
        {
            try
            {
                var report = await reportService.GenerateBonusDeterminationReportAsync(criteria);
                return Ok(report);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("export/{reportName}/pdf")]
        public async Task<IActionResult> GetPdfReport(string reportName)
        {
            try
            {
                var report = await reportService.GeneratePdfReport(reportName);
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
