namespace Application.DTOs.Maintenance
{
    public class MaintenanceRecordDto
    {
        public MaintenanceRecordDto()
        {
        }
        public MaintenanceRecordDto(int maintenanceRecordId, int deviceId, string deviceName, int technicianId, string technicianName, DateTime maintenanceDate, MaintenanceType maintenanceType, double cost, string description)
        {
            MaintenanceRecordId = maintenanceRecordId;
            DeviceId = deviceId;
            DeviceName = deviceName;
            TechnicianId = technicianId;
            TechnicianName = technicianName;
            MaintenanceDate = maintenanceDate;
            MaintenanceType = maintenanceType;
            Cost = cost;
            Description = description;
        }

        public int MaintenanceRecordId { get; set; }
        public int DeviceId { get; set; }
        public string DeviceName { get; set; } = string.Empty;
        public int TechnicianId { get; set; }
        public string TechnicianName { get; set; } = string.Empty;
        public DateTime MaintenanceDate { get; set; }
        public MaintenanceType MaintenanceType { get; set; }
        public double Cost { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}