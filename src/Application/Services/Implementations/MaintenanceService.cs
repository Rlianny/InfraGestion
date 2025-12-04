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
    private readonly IUnitOfWork unitOfWork;

    public MaintenanceService(IMaintenanceRecordRepository maintenanceRecordRepository,
        IDeviceRepository deviceRepository,
        ITechnicianRepository technicianRepository,
        IUnitOfWork unitOfWork)
    {
        this.technicianRepository = technicianRepository;
        this.deviceRepository = deviceRepository;
        this.maintenanceRecordRepository = maintenanceRecordRepository;
        this.unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<MaintenanceRecordDto>> GetAllMaintenanceHistoryAsync()
    {
        var maintenanceRecords = await maintenanceRecordRepository.GetAllAsync();
        var dtos = new List<MaintenanceRecordDto>();
        foreach (var maintenance in maintenanceRecords)
        {
            var device = await deviceRepository.GetByIdAsync(maintenance.DeviceId);
            var technician = await technicianRepository.GetByIdAsync(maintenance.TechnicianId);
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
                    MaintenanceRecordId = maintenance.MaintenanceRecordId,
                    DeviceId = maintenance.DeviceId,
                    TechnicianId = maintenance.TechnicianId,
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
            var device = await deviceRepository.GetByIdAsync(maintenance.DeviceId);
            var technician = await technicianRepository.GetByIdAsync(maintenance.TechnicianId);
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
                    MaintenanceRecordId = maintenance.MaintenanceRecordId,
                    DeviceId = maintenance.DeviceId,
                    TechnicianId = maintenance.TechnicianId,
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
        var device = await deviceRepository.GetByIdAsync(maintenance.DeviceId);
        var technician = await technicianRepository.GetByIdAsync(maintenance.TechnicianId);
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
            MaintenanceRecordId = maintenance.MaintenanceRecordId,
            DeviceId = maintenance.DeviceId,
            TechnicianId = maintenance.TechnicianId,
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
            var device = await deviceRepository.GetByIdAsync(maintenance.DeviceId);
            var technician = await technicianRepository.GetByIdAsync(maintenance.TechnicianId);
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
                    MaintenanceRecordId = maintenance.MaintenanceRecordId,
                    DeviceId = maintenance.DeviceId,
                    TechnicianId = maintenance.TechnicianId,
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
        // Obtener el dispositivo y cambiar su estado a UnderMaintenance
        var device = await deviceRepository.GetByIdAsync(recordDto.DeviceId)
            ?? throw new Exception($"Device with id {recordDto.DeviceId} not found");
        
        device.UpdateOperationalState(OperationalState.UnderMaintenance);
        await deviceRepository.UpdateAsync(device);

        MaintenanceRecord maintenanceRecord = new MaintenanceRecord(
            recordDto.TechnicianId,
            recordDto.DeviceId,
            recordDto.MaintenanceDate,
            recordDto.Cost,
            recordDto.MaintenanceType,
            recordDto.Description
        );
        await maintenanceRecordRepository.AddAsync(maintenanceRecord);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task CompleteMaintenanceAsync(int deviceId)
    {
        var device = await deviceRepository.GetByIdAsync(deviceId)
            ?? throw new Exception($"Device with id {deviceId} not found");
        
        if (device.OperationalState != OperationalState.UnderMaintenance)
        {
            throw new Exception($"Device with id {deviceId} is not under maintenance");
        }

        device.UpdateOperationalState(OperationalState.Operational);
        await deviceRepository.UpdateAsync(device);
        await unitOfWork.SaveChangesAsync();
    }
}
