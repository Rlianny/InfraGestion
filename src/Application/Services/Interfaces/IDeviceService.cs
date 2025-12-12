using Application.DTOs.DevicesDTOs;

namespace Application.Services.Interfaces
{
    /// <summary>
    /// Servicio responsable de las operaciones CRUD de dispositivos.
    /// </summary>
    public interface IDeviceService
    {
        #region Queries

        /// <summary>
        /// Obtiene todos los dispositivos filtrados según el rol del usuario autenticado.
        /// - Administrator/Director: Todos los dispositivos
        /// - SectionManager: Dispositivos de su sección
        /// - Technician: Dispositivos de su sección
        /// </summary>
        Task<IEnumerable<DeviceDto>> GetDevicesAsync(int currentUserId, string role, DeviceFilterDto? filter = null);

        /// <summary>
        /// Obtiene el detalle completo de un dispositivo.
        /// </summary>
        Task<DeviceDetailDto> GetDeviceDetailsAsync(int deviceId);

        /// <summary>
        /// Obtiene dispositivos de una sección específica (solo para Admin/Director/SectionManager).
        /// </summary>
        Task<IEnumerable<DeviceDto>> GetDevicesBySectionAsync(int sectionId);

        /// <summary>
        /// Obtiene dispositivos de la sección del usuario autenticado.
        /// </summary>
        Task<IEnumerable<DeviceDto>> GetMySectionDevicesAsync(int currentUserId);

        #endregion

        #region Commands

        /// <summary>
        /// Registra un nuevo dispositivo en el sistema.
        /// </summary>
        Task<int> RegisterDeviceAsync(RegisterDeviceDto request, int currentUserId);

        /// <summary>
        /// Actualiza la información de un dispositivo existente.
        /// </summary>
        Task UpdateDeviceAsync(int deviceId, UpdateDeviceRequestDto request);

        /// <summary>
        /// Elimina un dispositivo del sistema.
        /// </summary>
        Task DeleteDeviceAsync(int deviceId);

        /// <summary>
        /// Deshabilita un dispositivo.
        /// </summary>
        Task DisableDeviceAsync(int deviceId);

        #endregion
    }
}
