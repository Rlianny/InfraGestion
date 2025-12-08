namespace Application.DTOs.Personnel
{
    public class TechnicianDetailDto : TechnicianDto
    {
        public int TechnicianId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int YearsOfExperience { get; set; }
        public string Specialty { get; set; } = string.Empty;
        public IEnumerable<MaintenanceRecordDto> MaintenanceRecords { get; set; } = new List<MaintenanceRecordDto>();
        public IEnumerable<DecommissioningRequestDto> DecommissioningRequests { get; set; } = new List<DecommissioningRequestDto>();
    }
}