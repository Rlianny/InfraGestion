using Domain.Enums
namespace Application.DTOs.Inventory
{
    public class CreateDeviceRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public DeviceType DeviceType { get; set; }
        public DateTime AcquisitionDate { get; set; }
    }
}