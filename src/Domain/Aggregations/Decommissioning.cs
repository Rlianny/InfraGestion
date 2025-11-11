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
        public int DecommissioningId { get; private set; }
        public int DeviceReceiverId { get; private set; }
        public int DecommissioningRequestId { get; private set; }
        public int DeviceId { get; private set; }

        public DateTime DecommissioningDate { get; private set; }
        public DecommissioningReason Reason { get; private set; }

        public string? FinalDestination { get; private set; }
        public int ReceiverDepartmentId { get; private set; }

        private Decommissioning() { }
        public Decommissioning(int deviceId, int decommissioningRequestId, int deviceReceiverId, int receiverDepartmentId, DateTime decommissioningDate, DecommissioningReason reason, string finalDestination)
        {
            ValidateDecommissioningDate(decommissioningDate);
            DecommissioningDate = decommissioningDate;
            Reason = reason;
            FinalDestination = finalDestination;
            DeviceReceiverId = deviceReceiverId;
            DeviceId = deviceId;
            DecommissioningRequestId = decommissioningRequestId;
            ReceiverDepartmentId = receiverDepartmentId;
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
