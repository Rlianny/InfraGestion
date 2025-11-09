using Domain.Enums;

namespace Application.DTOs.Decommissioning
{
    public class DecommissioningDto
    {
        public DecommissioningDto(int decommissioningId, int deviceReceiverId, int decommissioningRequestId, int deviceId, DateTime decommissioningDate, DecommissioningReason reason, string? finalDestination, int receiverDepartmentId)
        {
            DecommissioningId = decommissioningId;
            DeviceReceiverId = deviceReceiverId;
            DecommissioningRequestId = decommissioningRequestId;
            DeviceId = deviceId;
            DecommissioningDate = decommissioningDate;
            Reason = reason;
            FinalDestination = finalDestination;
            ReceiverDepartmentId = receiverDepartmentId;
        }

        public int DecommissioningId { get; private set; }
        public int DeviceReceiverId { get; private set; }
        public int DecommissioningRequestId { get; private set; }
        public int DeviceId { get; private set; }

        public DateTime DecommissioningDate { get; private set; }
        public DecommissioningReason Reason { get; private set; }

        public string? FinalDestination { get; private set; }
        public int ReceiverDepartmentId { get; private set; }
    }
}