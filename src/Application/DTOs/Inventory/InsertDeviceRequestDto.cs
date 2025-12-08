using Domain.Enums;
namespace Application.DTOs.Inventory
{
    public class InsertDeviceRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public DeviceType DeviceType { get; set; }
        public DateTime AcquisitionDate { get; set; }
        public int technicianId { get; set; }
        public int userID { get; set; }
    }
}