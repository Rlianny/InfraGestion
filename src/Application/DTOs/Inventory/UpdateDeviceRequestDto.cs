namespace Application.DTOs.Inventory
{
    public class UpdateDeviceRequestDto
    {
        public int DeviceId { get; set; }
        public string? Name { get; set; }
        public DeviceType DeviceType { get; set; }
        public string? OperationalState { get; set; }
        public int? DepartmentId { get; set; }
    }
}