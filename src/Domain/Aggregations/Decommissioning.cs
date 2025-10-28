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
        public Decommissioning(int deviceID, int decommissioningRequestID, int deviceReceiverID, int receiverDepartmentID, DateTime decommissioningDate, DecommissioningReason reason, string finalDestination)
        {
            ValidateDecommissioningDate(decommissioningDate);
            DecommissioningDate = decommissioningDate;
            Reason = reason;
            FinalDestination = finalDestination;
            DeviceReceiverID = deviceReceiverID;
            DeviceID = deviceID;
            DecommissioningRequestID = decommissioningRequestID;
            ReceiverDepartmentID = receiverDepartmentID;
        }

        private void ValidateDecommissioningDate(DateTime date)
        {
            if (date > DateTime.Now)
            {
                throw new ArgumentException("Decommissioning date cannot be in the future");
            }
        }
        
        public bool IsIrreparable()
        {
            return Reason == DecommissioningReason.IrreparableTechnicalFailure;
        }
    }
}
