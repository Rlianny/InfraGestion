using Domain.Enums;
namespace Application.DTOs.Inventory
{
    public class DeviceDto
    {
        public int DeviceId { get; set; }
        public string Name { get; set; } = string.Empty;
        public DeviceType DeviceType { get; set; }
        public OperationalState OperationalState { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public int? SectionId { get; set; }
        public string? SectionName { get; set; }
        public DateTime AcquisitionDate { get; set; }
        public int MaintenanceCount { get; set; }
        public decimal TotalMaintenanceCost { get; set; }
        public DateTime? LastMaintenanceDate { get; set; }
    }
}