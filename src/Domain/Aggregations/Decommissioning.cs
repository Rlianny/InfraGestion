using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Aggregations
{
    public class Decommissioning
    {
        public int DecommissioningID { get; private set; }
        public int DeviceReceiverID { get; private set; }
        public int DecommissioningRequestID { get; private set; }
        public int DeviceID { get; private set; }

        public DateTime DecommissioningDate { get; private set; }
        public DecommissioningReason Reason { get; private set; }

        public string? FinalDestination { get; private set; }
        public int ReceiverDepartmentID { get; private set; }

        private Decommissioning() { }
        public Decommissioning(DateTime decommissioningDate, DecommissioningReason reason, string finalDestination, int deviceReceiverID, int deviceID, int decommissioningRequestID, int receiverDepartmentID)
        {
            DecommissioningDate = decommissioningDate;
            Reason = reason;
            FinalDestination = finalDestination;
            DeviceReceiverID = deviceReceiverID;
            DeviceID = deviceID;
            DecommissioningRequestID = decommissioningRequestID;
            ReceiverDepartmentID = receiverDepartmentID;
        }
    }
}
