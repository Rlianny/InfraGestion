using Domain.Enums;
namespace Application.DTOs.Inventory
{
    public class InsertDeviceRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public DeviceType DeviceType { get; set; }
        public DateTime AcquisitionDate { get; set; }
        public int assignedTechnician { get; set; }
        public int admin { get; set; }
    }
}