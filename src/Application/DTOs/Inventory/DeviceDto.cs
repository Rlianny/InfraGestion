using Domain.Enums;
namespace Application.DTOs.Inventory
{
    public class DeviceDto
    {
        public DeviceDto(int deviceId, string name, DeviceType deviceType, OperationalState operationalState, string departmentName)
        {
            DeviceId = deviceId;
            Name = name;
            DeviceType = deviceType;
            OperationalState = operationalState;
            DepartmentName = departmentName;         
        }

        public int DeviceId { get; set; }
        public string Name { get; set; } = string.Empty;
        public DeviceType DeviceType { get; set; }
        public OperationalState OperationalState { get; set; }
        public string DepartmentName { get; set; } = string.Empty;

    }
}