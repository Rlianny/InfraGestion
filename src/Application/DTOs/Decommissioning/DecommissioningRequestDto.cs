using Domain.Enums;
namespace Application.DTOs.Decommissioning
{
    public class DecommissioningRequestDto
    {
        public int DecommissioningRequestId { get; set; }
        public int DeviceId { get; set; }
        public string DeviceName { get; set; } = string.Empty;
        public int TechnicianId { get; set; }
        public string TechnicianName { get; set; } = string.Empty;
        public int DeviceReceiverId { get; set; }
        public string DeviceReceiverName { get; set; } = string.Empty;
        public DateTime RequestDate { get; set; }
        public DecommissioningStatus Status { get; set; } = DecommissioningStatus.Pending; // Pending, Approved, Rejected
        public string Justification { get; set; } = string.Empty;
        public DecommissioningReason Reason { get; set; }
        public string? ReasonDescription { get; set; }
        public DateTime? ReviewedDate { get; set; }
        public int? ReviewedByUserId { get; set; }
        public string? ReviewedByUserName { get; set; }
    }
}