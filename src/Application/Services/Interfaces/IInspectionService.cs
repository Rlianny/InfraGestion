using Application.DTOs.DevicesDTOs;
using Application.DTOs.InspectionDTOs;

namespace Application.Services.Interfaces
{
    /// <summary>
    /// Servicio responsable de las operaciones de inspección de dispositivos.
    /// </summary>
    public interface IInspectionService
    {
        #region Queries - Technician

        /// <summary>
        /// Obtiene todas las solicitudes de inspección asignadas a un técnico.
        /// </summary>
        Task<IEnumerable<ReceivingInspectionRequestDto>> GetInspectionRequestsByTechnicianAsync(int technicianId);

        /// <summary>
        /// Obtiene las solicitudes de inspección pendientes de un técnico.
        /// </summary>
        Task<IEnumerable<ReceivingInspectionRequestDto>> GetPendingInspectionsByTechnicianAsync(int technicianId);

        #endregion

        #region Queries - Administrator

        /// <summary>
        /// Obtiene las solicitudes de inspección creadas por un administrador.
        /// </summary>
        Task<IEnumerable<ReceivingInspectionRequestDto>> GetInspectionRequestsByAdminAsync(int adminId);

        /// <summary>
        /// Obtiene los dispositivos revisados asignados por un administrador.
        /// </summary>
        Task<IEnumerable<DeviceDto>> GetRevisedDevicesByAdminAsync(int adminId);

        #endregion

        #region Commands

        /// <summary>
        /// Asigna un dispositivo para inspección/revisión.
        /// </summary>
        Task AssignDeviceForInspectionAsync(AssignDeviceForInspectionRequestDto request);

        /// <summary>
        /// Procesa la decisión de inspección (aprobar/rechazar).
        /// </summary>
        Task ProcessInspectionDecisionAsync(InspectionDecisionRequestDto request);

        #endregion
    }
}
