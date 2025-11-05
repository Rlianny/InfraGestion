using Application.DTOs.Maintenance;
using Application.Services.Interfaces;
using Domain.Aggregations;
using Domain.Interfaces;

public class MaintenanceService : IMaintenanceService
{
    private readonly IMaintenanceRecordRepository maintenanceRecordRepository;
    private readonly IDeviceRepository deviceRepository;
    private readonly IUserRepository userRepository;

    public MaintenanceService(
        IMaintenanceRecordRepository maintenanceRecordRepository,
        IDeviceRepository deviceRepository,
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
                    MaintenanceRecordId = maintenance.MaintenanceRecordID,
                    DeviceId = maintenance.DeviceID,
                    TechnicianId = maintenance.TechnicianID,
                    MaintenanceDate = maintenance.Date,
                    MaintenanceType = maintenance.Type,
                    DeviceName =
                        (await deviceRepository.GetByIdAsync(maintenance.DeviceID))?.Name
                        ?? "Unknown",
                    TechnicianName =
                        (await userRepository.GetByIdAsync(maintenance.TechnicianID))?.FullName
                        ?? "Unknown",
                }
            );
        }
        return dtos;
    }

    public async Task<IEnumerable<MaintenanceRecordDto>> GetDeviceMaintenanceHistoryAsync(
        int deviceID
    )
    {
        var maintenanceRecords = await maintenanceRecordRepository.GetMaintenancesByDeviceAsync(
            deviceID
        );
        var dtos = new List<MaintenanceRecordDto>();
        foreach (var maintenance in maintenanceRecords)
        {
            dtos.Add(
                new MaintenanceRecordDto
                {
                    Cost = maintenance.Cost,
                    Description = maintenance.Description,
                    MaintenanceRecordId = maintenance.MaintenanceRecordID,
                    DeviceId = maintenance.DeviceID,
                    TechnicianId = maintenance.TechnicianID,
                    MaintenanceDate = maintenance.Date,
                    MaintenanceType = maintenance.Type,
                    DeviceName =
                        (await deviceRepository.GetByIdAsync(maintenance.DeviceID))?.Name
                        ?? "Unknown",
                    TechnicianName =
                        (await userRepository.GetByIdAsync(maintenance.TechnicianID))?.FullName
                        ?? "Unknown",
                }
            );
        }
        return dtos;
    }

    public async Task<MaintenanceRecordDto> GetMaintenanceRecordAsync(int maintenanceID)
    {
        var maintenance = await maintenanceRecordRepository.GetByIdAsync(maintenanceID);
        if (maintenance == null)
        {
            throw new Exception("Maintenance record not found");
        }
        return new MaintenanceRecordDto
        {
            Cost = maintenance.Cost,
            Description = maintenance.Description,
            MaintenanceRecordId = maintenance.MaintenanceRecordID,
            DeviceId = maintenance.DeviceID,
            TechnicianId = maintenance.TechnicianID,
            MaintenanceDate = maintenance.Date,
            MaintenanceType = maintenance.Type,
            DeviceName =
                (await deviceRepository.GetByIdAsync(maintenance.DeviceID))?.Name ?? "Unknown",
            TechnicianName =
                (await userRepository.GetByIdAsync(maintenance.TechnicianID))?.FullName
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
                    MaintenanceRecordId = maintenance.MaintenanceRecordID,
                    DeviceId = maintenance.DeviceID,
                    TechnicianId = maintenance.TechnicianID,
                    MaintenanceDate = maintenance.Date,
                    MaintenanceType = maintenance.Type,
                    DeviceName =
                        (await deviceRepository.GetByIdAsync(maintenance.DeviceID))?.Name
                        ?? "Unknown",
                    TechnicianName =
                        (await userRepository.GetByIdAsync(maintenance.TechnicianID))?.FullName
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
