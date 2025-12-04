using Application.DTOs.Maintenance;

namespace Application.Services.Interfaces
{
    public interface IMaintenanceService
    {
        Task RegisterMaintenanceAsync(MaintenanceRecordDto recordDto);
        Task CompleteMaintenanceAsync(int deviceId);
        Task<IEnumerable<MaintenanceRecordDto>> GetDeviceMaintenanceHistoryAsync(int deviceId);
        Task<IEnumerable<MaintenanceRecordDto>> GetTechnicianMaintenanceHistoryAsync(int technicianId);
        Task<IEnumerable<MaintenanceRecordDto>> GetAllMaintenanceHistoryAsync();
        Task<MaintenanceRecordDto> GetMaintenanceRecordAsync(int maintenanceId);
    }
}