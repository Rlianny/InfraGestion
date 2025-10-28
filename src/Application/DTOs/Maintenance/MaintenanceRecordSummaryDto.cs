namespace Application.DTOs.Maintenance
{
    public class MaintenanceRecordSummaryDto
    {
        public int MaintenanceRecordId { get; set; }
        public DateTime MaintenanceDate { get; set; }
        public MaintenanceType MaintenanceType { get; set; }
        public decimal Cost { get; set; }
        public string TechnicianName { get; set; } = string.Empty;
    }
}