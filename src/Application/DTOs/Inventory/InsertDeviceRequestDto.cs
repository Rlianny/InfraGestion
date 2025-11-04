using Domain.Enums;
namespace Application.DTOs.Inventory
{
    public class InsertDeviceRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public int DepartmentID { get; set; }
        public DeviceType DeviceType { get; set; }
        public DateTime AcquisitionDate { get; set; }
    }
}