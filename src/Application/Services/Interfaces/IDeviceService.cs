using Application.DTOs.Inventory;

namespace Application.Services.Interfaces
{
    /// <summary>
    /// Servicio responsable de las operaciones CRUD de dispositivos.
    /// </summary>
    public interface IDeviceService
    {
        #region Queries

        /// <summary>
        /// Obtiene todos los dispositivos según el rol del usuario.
        /// </summary>
        Task<IEnumerable<DeviceDto>> GetAllDevicesAsync(int userId);

        /// <summary>
        /// Obtiene dispositivos filtrados según criterios específicos.
        /// </summary>
        Task<IEnumerable<DeviceDto>> GetDevicesByFilterAsync(DeviceFilterDto filter, int userId);

        /// <summary>
        /// Obtiene el detalle completo de un dispositivo.
        /// </summary>
        Task<DeviceDetailDto> GetDeviceDetailsAsync(int deviceId);

        /// <summary>
        /// Obtiene dispositivos de una sección específica.
        /// </summary>
        Task<IEnumerable<DeviceDto>> GetDevicesBySectionAsync(int userId, int sectionId);

        /// <summary>
        /// Obtiene dispositivos de la sección del usuario.
        /// </summary>
        Task<IEnumerable<DeviceDto>> GetSectionDevicesByUserAsync(int userId);

        #endregion

        #region Commands

        /// <summary>
        /// Registra un nuevo dispositivo en el sistema.
        /// </summary>
        Task RegisterDeviceAsync(RegisterNewDeviceDto request);

        /// <summary>
        /// Actualiza la información de un dispositivo existente.
        /// </summary>
        Task UpdateDeviceAsync(UpdateDeviceRequestDto request);

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
