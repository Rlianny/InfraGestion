namespace Application.DTOs.InspectionDTOs
{
    public class AssignDeviceForInspectionRequestDto
    {
        public int DeviceId { get; set; }
        public int TechnicianId { get; set; }
        public int AdministratorId { get; set; }
    }
}