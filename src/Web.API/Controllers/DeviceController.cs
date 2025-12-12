using Application.DTOs.DevicesDTOs;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.API.Shared;

namespace Web.API.Controllers
{
    /// <summary>
    /// Controlador responsable de las operaciones CRUD de dispositivos.
    /// Los dispositivos se filtran automáticamente según el rol del usuario autenticado:
    /// - Administrator/Director: Acceso a todos los dispositivos
    /// - SectionManager/Technician: Acceso solo a dispositivos de su sección
    /// </summary>
    [ApiController]
    [Route("api/devices")]
    [Authorize]
    public class DeviceController : BaseApiController
    {
        private readonly IDeviceService _deviceService;

        public DeviceController(IDeviceService deviceService)
        {
            _deviceService = deviceService;
        }

        #region GET

        /// <summary>
        /// Obtiene el inventario de dispositivos filtrado según el rol del usuario autenticado.
        /// Opcionalmente se pueden aplicar filtros adicionales por tipo, estado y departamento.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<DeviceDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDevicesAsync([FromQuery] DeviceFilterDto? filter)
        {
            var currentUserId = GetCurrentUserId();
            var role = GetCurrentUserRole();
            
            var devices = await _deviceService.GetDevicesAsync(currentUserId, role, filter);
            return Ok(devices);
        }

        /// <summary>
        /// Obtiene el detalle completo de un dispositivo específico.
        /// </summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<DeviceDetailDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDeviceDetailsAsync(int id)
        {
            var deviceDetail = await _deviceService.GetDeviceDetailsAsync(id);
            return Ok(deviceDetail);
        }

        /// <summary>
        /// Obtiene dispositivos de la sección del usuario autenticado.
        /// </summary>
        [HttpGet("my-section")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<DeviceDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMySectionDevicesAsync()
        {
            var currentUserId = GetCurrentUserId();
            var devices = await _deviceService.GetMySectionDevicesAsync(currentUserId);
            return Ok(devices);
        }

        /// <summary>
        /// Obtiene dispositivos de una sección específica.
        /// Solo accesible para Administrator, Director y SectionManager.
        /// </summary>
        [HttpGet("sections/{sectionId:int}")]
        [Authorize(Roles = "Administrator,Director,SectionManager")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<DeviceDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDevicesBySectionAsync(int sectionId)
        {
            var devices = await _deviceService.GetDevicesBySectionAsync(sectionId);
            return Ok(devices);
        }

        #endregion

        #region POST

        /// <summary>
        /// Registra un nuevo dispositivo en el sistema.
        /// Solo accesible para Administrator.
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterDeviceAsync([FromBody] RegisterDeviceDto request)
        {
            var currentUserId = GetCurrentUserId();
            var deviceId = await _deviceService.RegisterDeviceAsync(request, currentUserId);
            return CreatedAtAction(nameof(GetDeviceDetailsAsync), new { id = deviceId }, deviceId);
        }

        /// <summary>
        /// Deshabilita un dispositivo.
        /// Solo accesible para Administrator.
        /// </summary>
        [HttpPost("{id:int}/disable")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DisableDeviceAsync(int id)
        {
            await _deviceService.DisableDeviceAsync(id);
            return Ok("Device disabled successfully");
        }

        #endregion

        #region PUT

        /// <summary>
        /// Actualiza la información de un dispositivo existente.
        /// Solo accesible para Administrator.
        /// </summary>
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateDeviceAsync(int id, [FromBody] UpdateDeviceRequestDto request)
        {
            await _deviceService.UpdateDeviceAsync(id, request);
            return Ok("Device updated successfully");
        }

        #endregion

        #region DELETE

        /// <summary>
        /// Elimina un dispositivo del sistema.
        /// Solo accesible para Administrator.
        /// </summary>
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteDeviceAsync(int id)
        {
            await _deviceService.DeleteDeviceAsync(id);
            return NoContent();
        }

        #endregion
    }
}
