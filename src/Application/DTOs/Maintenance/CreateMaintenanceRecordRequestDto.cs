namespace Application.DTOs.Maintenance
{
    public class CreateMaintenanceRecordRequestDto
    {
        public int DeviceId { get; set; }
        public DateTime MaintenanceDate { get; set; }
        public MaintenanceType MaintenanceType { get; set; } = MaintenanceType.Preventive;
        public decimal Cost { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}