using Application.DTOs.Maintenance;

namespace Application.Services.Interfaces
{
    public interface IMaintenanceService
    {
        Task RegisterMaintenanceAsync(MaintenanceRecordDto recordDto);
        Task<IEnumerable<MaintenanceRecordDto>> GetDeviceMaintenanceHistoryAsync(int deviceID);
        Task<IEnumerable<MaintenanceRecordDto>> GetTechnicianMaintenanceHistoryAsync(int technicianId);
        Task<IEnumerable<MaintenanceRecordDto>> GetAllMaintenanceHistoryAsync();
        Task<MaintenanceRecordDto> GetMaintenanceRecordAsync(int maintenanceID);
    }
}