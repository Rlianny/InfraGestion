namespace Application.DTOs.Inventory
{
    public class AssignDeviceForInspectionRequestDto
    {
        public int DeviceId { get; set; }
        public int TechnicianId { get; set; }
    }
}