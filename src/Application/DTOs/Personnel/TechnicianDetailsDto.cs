using Application.DTOs.Maintenance;
using Application.DTOs.Decommissioning;

namespace Application.DTOs.Personnel
{
    public class TechnicianDetailsDto : TechnicianDto
    {
        public double AverageRating { get; set; }
        public DateTime? LastInterventionDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public string SectionName { get; set; } = string.Empty;
        public string SectionManagerName { get; set; } = string.Empty;
        public IEnumerable<RateDto> Ratings { get; set; } = new List<RateDto>();
        public IEnumerable<MaintenanceRecordDto> MaintenanceRecords { get; set; } = new List<MaintenanceRecordDto>();
        public IEnumerable<DecommissioningRequestDto> DecommissioningRequests { get; set; } = new List<DecommissioningRequestDto>();
    }
}