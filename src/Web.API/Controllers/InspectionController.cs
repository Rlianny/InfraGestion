using Application.DTOs.DevicesDTOs;
using Application.DTOs.InspectionDTOs;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.API.Shared;

namespace Web.API.Controllers
{
    /// <summary>
    /// Controlador responsable de las operaciones de inspección de dispositivos.
    /// </summary>
    [ApiController]
    [Route("api/inspections")]
    public class InspectionController : BaseApiController
    {
        private readonly IInspectionService _inspectionService;

        public InspectionController(IInspectionService inspectionService)
        {
            _inspectionService = inspectionService;
        }

        #region GET

        /// <summary>
        /// Obtiene las solicitudes de inspección asignadas a un técnico.
        /// </summary>
        [HttpGet("technician/{technicianId:int}/requests")]
        [Authorize(Roles = "Technician")]
        [ProducesResponseType(
            typeof(ApiResponse<IEnumerable<ReceivingInspectionRequestDto>>),
            StatusCodes.Status200OK
        )]
        public async Task<IActionResult> GetInspectionRequestsByTechnicianAsync(int technicianId)
        {
            var inspectionRequests = await _inspectionService.GetInspectionRequestsByTechnicianAsync(
                technicianId
            );
            return Ok(inspectionRequests);
        }

        /// <summary>
        /// Obtiene las solicitudes de inspección pendientes para un técnico.
        /// </summary>
        [HttpGet("technician/{technicianId:int}/pending")]
        [Authorize(Roles = "Technician")]
        [ProducesResponseType(
            typeof(ApiResponse<IEnumerable<ReceivingInspectionRequestDto>>),
            StatusCodes.Status200OK
        )]
        public async Task<IActionResult> GetPendingInspectionsByTechnicianAsync(int technicianId)
        {
            var pendingInspections = await _inspectionService.GetPendingInspectionsByTechnicianAsync(
                technicianId
            );
            return Ok(pendingInspections);
        }

        /// <summary>
        /// Obtiene los dispositivos revisados por un administrador.
        /// </summary>
        [HttpGet("admin/{adminId:int}/revised-devices")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<DeviceDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRevisedDevicesByAdminAsync(int adminId)
        {
            var devices = await _inspectionService.GetRevisedDevicesByAdminAsync(adminId);
            return Ok(devices);
        }

        /// <summary>
        /// Obtiene las solicitudes de inspección inicial por administrador.
        /// </summary>
        [HttpGet("admin/{adminId:int}/requests")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(
            typeof(ApiResponse<IEnumerable<ReceivingInspectionRequestDto>>),
            StatusCodes.Status200OK
        )]
        public async Task<IActionResult> GetInspectionRequestsByAdminAsync(int adminId)
        {
            var requests = await _inspectionService.GetInspectionRequestsByAdminAsync(
                adminId
            );
            return Ok(requests);
        }

        #endregion

        #region POST

        /// <summary>
        /// Procesa la decisión de inspección de un técnico (aprobar/rechazar).
        /// </summary>
        [HttpPost("decision")]
        [Authorize(Roles = "Technician")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ProcessInspectionDecisionAsync(
            [FromBody] InspectionDecisionRequestDto request
        )
        {
            await _inspectionService.ProcessInspectionDecisionAsync(request);
            var message = request.IsApproved
                ? "Device approved successfully"
                : "Device rejected successfully";
            return Ok(message);
        }

        /// <summary>
        /// Asigna un dispositivo para revisión/inspección.
        /// </summary>
        [HttpPost("assign")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AssignDeviceForInspectionAsync(
            [FromBody] AssignDeviceForInspectionRequestDto request
        )
        {
            await _inspectionService.AssignDeviceForInspectionAsync(request);
            return Ok("Device assigned for inspection successfully");
        }

        #endregion
    }
}
