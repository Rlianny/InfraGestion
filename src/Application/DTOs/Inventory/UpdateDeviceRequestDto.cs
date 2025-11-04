using Domain.Enums;
namespace Application.DTOs.Inventory
{
    public class UpdateDeviceRequestDto
    {
        public int DeviceId { get; set; }
        public string Name { get; set; }
        public DeviceType DeviceType { get; set; }
        public OperationalState OperationalState { get; set; }
        public int DepartmentId { get; set; }
    }
}