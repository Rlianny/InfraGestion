using Application.DTOs.Maintenance;
using Application.Services.Interfaces;
using Domain.Aggregations;
using Domain.Enums;
using Domain.Interfaces;

public class MaintenanceService : IMaintenanceService
{
    private readonly IMaintenanceRecordRepository maintenanceRecordRepository;
    private readonly IDeviceRepository deviceRepository;
    private readonly ITechnicianRepository technicianRepository;
    public MaintenanceService(IMaintenanceRecordRepository maintenanceRecordRepository,
        IDeviceRepository deviceRepository,
     ITechnicianRepository technicianRepository)
    {
        this.technicianRepository = technicianRepository;
        this.deviceRepository = deviceRepository;
        this.maintenanceRecordRepository = maintenanceRecordRepository;
    }

    public async Task<IEnumerable<MaintenanceRecordDto>> GetAllMaintenanceHistoryAsync()
    {
        var maintenanceRecords = await maintenanceRecordRepository.GetAllAsync();
        var dtos = new List<MaintenanceRecordDto>();
        foreach (var maintenance in maintenanceRecords)
        {
            var device = await deviceRepository.GetByIdAsync(maintenance.DeviceID);
            var technician = await technicianRepository.GetByIdAsync(maintenance.TechnicianID);
            if (device == null)
            {
                throw new Exception("There is a maintenance with no device associated");
            }
            if (technician == null)
            {
                throw new Exception("There is a maintenance with no technician associated");
            }
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
                    TechnicianName = technician.FullName,
                    DeviceName = device.Name
                }
            );
        }
        return dtos;
    }

    public async Task<IEnumerable<MaintenanceRecordDto>> GetDeviceMaintenanceHistoryAsync(
        int deviceID
    )
    {
        var maintenanceRecords = await maintenanceRecordRepository.GetMaintenancesByDeviceAsync(deviceID);
        var dtos = new List<MaintenanceRecordDto>();
        foreach (var maintenance in maintenanceRecords)
        {
            var device = await deviceRepository.GetByIdAsync(maintenance.DeviceID);
            var technician = await technicianRepository.GetByIdAsync(maintenance.TechnicianID);
            if (device == null)
            {
                throw new Exception("There is a maintenance with no device associated");
            }
            if (technician == null)
            {
                throw new Exception("There is a maintenance with no technician associated");
            }
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
                    DeviceName = device.Name,
                    TechnicianName = technician.FullName
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
        var device = await deviceRepository.GetByIdAsync(maintenance.DeviceID);
        var technician = await technicianRepository.GetByIdAsync(maintenance.TechnicianID);
        if (device == null)
        {
            throw new Exception("There is a maintenance with no device associated");
        }
        if (technician == null)
        {
            throw new Exception("There is a maintenance with no technician associated");
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
            DeviceName = device.Name,
            TechnicianName = technician.FullName
        };
    }

    public async Task<IEnumerable<MaintenanceRecordDto>> GetTechnicianMaintenanceHistoryAsync(int technicianId)
    {
        var maintenances = await maintenanceRecordRepository.GetMaintenancesByTechnicianAsync(technicianId);
        var dtos = new List<MaintenanceRecordDto>();
        foreach (var maintenance in maintenances)
        {
            var device = await deviceRepository.GetByIdAsync(maintenance.DeviceID);
            var technician = await technicianRepository.GetByIdAsync(maintenance.TechnicianID);
            if (device == null)
            {
                throw new Exception("There is a maintenance with no device associated");
            }
            if (technician == null)
            {
                throw new Exception("There is a maintenance with no technician associated");
            }
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
                    DeviceName = device.Name,
                    TechnicianName = technician.FullName
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
