using Domain.Enums;

namespace Application.DTOs.Decommissioning
{
    public class CreateDecommissioningRequestDto
    {
        public int DeviceId { get; set; }
        public int TechnicianId { get; set; }
        public DecommissioningReason Reason { get; set; }
        public DateTime RequestDate { get; set; }

    }
}