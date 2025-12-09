using Application.DTOs.Maintenance;
using Application.DTOs.Decommissioning;

namespace Application.DTOs.Personnel
{
    public class TechnicianDetailDto : TechnicianDto
    {
        public IEnumerable<MaintenanceRecordDto> MaintenanceRecords { get; set; } = new List<MaintenanceRecordDto>();
        public IEnumerable<DecommissioningRequestDto> DecommissioningRequests { get; set; } = new List<DecommissioningRequestDto>();
    }
}