using Application.DTOs.Maintenance;
using Application.Services.Interfaces;
using Domain.Aggregations;
using Domain.Interfaces;

public class MaintenanceService : IMaintenanceService
{
    private readonly IMaintenanceRecordRepository maintenanceRecordRepository;
    private readonly IdeviceRepository deviceRepository;
    private readonly IUserRepository userRepository;

    public MaintenanceService(
        IMaintenanceRecordRepository maintenanceRecordRepository,
        IdeviceRepository deviceRepository,
        IUserRepository userRepository
    )
    {
        this.userRepository = userRepository;
        this.deviceRepository = deviceRepository;
        this.maintenanceRecordRepository = maintenanceRecordRepository;
    }

    public async Task<IEnumerable<MaintenanceRecordDto>> GetAllMaintenanceHistoryAsync()
    {
        var maintenanceRecords = await maintenanceRecordRepository.GetAllAsync();
        var dtos = new List<MaintenanceRecordDto>();
        foreach (var maintenance in maintenanceRecords)
        {
            dtos.Add(
                new MaintenanceRecordDto
                {
                    Cost = maintenance.Cost,
                    Description = maintenance.Description,
                    MaintenanceRecordId = maintenance.MaintenanceRecordId,
                    DeviceId = maintenance.DeviceId,
                    TechnicianId = maintenance.TechnicianId,
                    MaintenanceDate = maintenance.Date,
                    MaintenanceType = maintenance.Type,
                    DeviceName =
                        (await deviceRepository.GetByIdAsync(maintenance.DeviceId))?.Name
                        ?? "Unknown",
                    TechnicianName =
                        (await userRepository.GetByIdAsync(maintenance.TechnicianId))?.FullName
                        ?? "Unknown",
                }
            );
        }
        return dtos;
    }

    public async Task<IEnumerable<MaintenanceRecordDto>> GetDeviceMaintenanceHistoryAsync(
        int deviceId
    )
    {
        var maintenanceRecords = await maintenanceRecordRepository.GetMaintenancesByDeviceAsync(
            deviceId
        );
        var dtos = new List<MaintenanceRecordDto>();
        foreach (var maintenance in maintenanceRecords)
        {
            dtos.Add(
                new MaintenanceRecordDto
                {
                    Cost = maintenance.Cost,
                    Description = maintenance.Description,
                    MaintenanceRecordId = maintenance.MaintenanceRecordId,
                    DeviceId = maintenance.DeviceId,
                    TechnicianId = maintenance.TechnicianId,
                    MaintenanceDate = maintenance.Date,
                    MaintenanceType = maintenance.Type,
                    DeviceName =
                        (await deviceRepository.GetByIdAsync(maintenance.DeviceId))?.Name
                        ?? "Unknown",
                    TechnicianName =
                        (await userRepository.GetByIdAsync(maintenance.TechnicianId))?.FullName
                        ?? "Unknown",
                }
            );
        }
        return dtos;
    }

    public async Task<MaintenanceRecordDto> GetMaintenanceRecordAsync(int maintenanceId)
    {
        var maintenance = await maintenanceRecordRepository.GetByIdAsync(maintenanceId);
        if (maintenance == null)
        {
            throw new Exception("Maintenance record not found");
        }
        return new MaintenanceRecordDto
        {
            Cost = maintenance.Cost,
            Description = maintenance.Description,
            MaintenanceRecordId = maintenance.MaintenanceRecordId,
            DeviceId = maintenance.DeviceId,
            TechnicianId = maintenance.TechnicianId,
            MaintenanceDate = maintenance.Date,
            MaintenanceType = maintenance.Type,
            DeviceName =
                (await deviceRepository.GetByIdAsync(maintenance.DeviceId))?.Name ?? "Unknown",
            TechnicianName =
                (await userRepository.GetByIdAsync(maintenance.TechnicianId))?.FullName
                ?? "Unknown",
        };
    }

    public async Task<IEnumerable<MaintenanceRecordDto>> GetTechnicianMaintenanceHistoryAsync(
        int technicianId
    )
    {
        var maintenances = await maintenanceRecordRepository.GetMaintenancesByTechnicianAsync(
            technicianId
        );
        var dtos = new List<MaintenanceRecordDto>();
        foreach (var maintenance in maintenances)
        {
            dtos.Add(
                new MaintenanceRecordDto
                {
                    Cost = maintenance.Cost,
                    Description = maintenance.Description,
                    MaintenanceRecordId = maintenance.MaintenanceRecordId,
                    DeviceId = maintenance.DeviceId,
                    TechnicianId = maintenance.TechnicianId,
                    MaintenanceDate = maintenance.Date,
                    MaintenanceType = maintenance.Type,
                    DeviceName =
                        (await deviceRepository.GetByIdAsync(maintenance.DeviceId))?.Name
                        ?? "Unknown",
                    TechnicianName =
                        (await userRepository.GetByIdAsync(maintenance.TechnicianId))?.FullName
                        ?? "Unknown",
                }
            );
        }
        return dtos;
    }

    public async Task RegisterMaintenanceAsync(MaintenanceRecordDto recordDto)
    {
        MaintenanceRecord maintenanceRecord = new MaintenanceRecord(
            recordDto.TechnicianId,
            recordDto.DeviceId,
            recordDto.MaintenanceDate,
            recordDto.Cost,
            recordDto.MaintenanceType,
            recordDto.Description
        );
        await maintenanceRecordRepository.AddAsync(maintenanceRecord);
    }
}
