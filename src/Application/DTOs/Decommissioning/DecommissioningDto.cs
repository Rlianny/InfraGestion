using Domain.Enums;

namespace Application.DTOs.Decommissioning
{
    public class DecommissioningDto
    {
        public DecommissioningDto(int decommissioningID, int deviceReceiverID, int decommissioningRequestID, int deviceID, DateTime decommissioningDate, DecommissioningReason reason, string? finalDestination, int receiverDepartmentID)
        {
            DecommissioningID = decommissioningID;
            DeviceReceiverID = deviceReceiverID;
            DecommissioningRequestID = decommissioningRequestID;
            DeviceID = deviceID;
            DecommissioningDate = decommissioningDate;
            Reason = reason;
            FinalDestination = finalDestination;
            ReceiverDepartmentID = receiverDepartmentID;
        }

        public int DecommissioningID { get; private set; }
        public int DeviceReceiverID { get; private set; }
        public int DecommissioningRequestID { get; private set; }
        public int DeviceID { get; private set; }

        public DateTime DecommissioningDate { get; private set; }
        public DecommissioningReason Reason { get; private set; }

        public string? FinalDestination { get; private set; }
        public int ReceiverDepartmentID { get; private set; }
    }
}