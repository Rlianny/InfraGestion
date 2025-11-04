namespace Application.DTOs.Maintenance
{
    public class MaintenanceRecordDto
    {
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