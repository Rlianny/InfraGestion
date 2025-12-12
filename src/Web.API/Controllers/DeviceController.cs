using Application.DTOs.Inventory;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.API.Shared;

namespace Web.API.Controllers
{
    /// <summary>
    /// Controlador responsable de las operaciones CRUD de dispositivos.
    /// </summary>
    [ApiController]
    [Route("api/devices")]
    public class DeviceController : BaseApiController
    {
        private readonly IDeviceService _deviceService;

        public DeviceController(IDeviceService deviceService)
        {
            _deviceService = deviceService;
        }

        #region GET

        /// <summary>
        /// Obtiene el inventario completo de la compañía.
        /// </summary>
        [HttpGet("user/{userId:int}")]
        [Authorize(Roles = "Director")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<DeviceDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllDevicesAsync(int userId)
        {
            var devices = await _deviceService.GetAllDevicesAsync(userId);
            return Ok(devices);
        }

        /// <summary>
        /// Obtiene el inventario de una sección específica.
        /// </summary>
        [HttpGet("user/{userId:int}/sections/{sectionId:int}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<DeviceDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDevicesBySectionAsync(int userId, int sectionId)
        {
            var devices = await _deviceService.GetDevicesBySectionAsync(userId, sectionId);
            return Ok(devices);
        }

        /// <summary>
        /// Obtiene el inventario de la sección del usuario autenticado.
        /// </summary>
        [HttpGet("user/{userId:int}/my-section-devices")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<DeviceDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSectionDevicesByUser(int userId)
        {
            var devices = await _deviceService.GetSectionDevicesByUserAsync(userId);
            return Ok(devices);
        }

        /// <summary>
        /// Obtiene el detalle de un dispositivo específico.
        /// </summary>
        [HttpGet("{id:int}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<DeviceDetailDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDeviceDetailsAsync(int id)
        {
            var deviceDetail = await _deviceService.GetDeviceDetailsAsync(id);
            return Ok(deviceDetail);
        }

        #endregion

        #region POST

        /// <summary>
        /// Registra un nuevo dispositivo en el sistema.
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterDeviceAsync(
            [FromBody] RegisterNewDeviceDto request
        )
        {
            await _deviceService.RegisterDeviceAsync(request);
            return StatusCode(StatusCodes.Status201Created, "Device registered successfully");
        }

        /// <summary>
        /// Deshabilita un dispositivo.
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
        /// </summary>
        [HttpPut]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateDeviceAsync(
            [FromBody] UpdateDeviceRequestDto request
        )
        {
            await _deviceService.UpdateDeviceAsync(request);
            return Ok("Device updated successfully");
        }

        #endregion

        #region DELETE

        /// <summary>
        /// Elimina un dispositivo del sistema.
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
