using Domain.Enums;

namespace Application.DTOs.Decommissioning
{
    public class DecommissioningDto
    {
        public int DeviceId { get; set; }
        public string DeviceName { get; set; } = string.Empty;
        public int DecommissioningRequestId { get; set; }
        public int DeviceReceiverId { get; set; }
        public string DeviceReceiverName { get; set; } = string.Empty;
        public int ReceiverDepartmentId { get; set; }
        public string ReceiverDepartmentName { get; set; } = string.Empty;
        public DateTime DecommissioningDate { get; set; }
        public DecommissioningReason Reason { get; set; }
        public string? FinalDestination { get; set; }
    }
}