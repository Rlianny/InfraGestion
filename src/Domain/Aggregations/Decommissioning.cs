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

        public virtual DeviceReceiver? DeviceReceiver { get; private set; }
        public virtual Device? Device { get; private set; }
        public virtual DecommissioningRequest? DecommissioningRequest { get; private set; }
        public virtual Department? Department { get; private set; }

        private Decommissioning() { }
        public Decommissioning(Device device, DecommissioningRequest decommissioningRequest, DeviceReceiver deviceReceiver, Department receiverDepartment, DateTime decommissioningDate, DecommissioningReason reason, string finalDestination)
        {
            Device = device;
            DeviceReceiver = deviceReceiver;
            DecommissioningRequest = decommissioningRequest;
            DecommissioningDate = decommissioningDate;
            Reason = reason;
            FinalDestination = finalDestination;
            DeviceReceiverID = deviceReceiver.UserID;
            DeviceID = device.DeviceID;
            DecommissioningRequestID = decommissioningRequest.DecommissioningRequestID;
            Department = receiverDepartment;
            ReceiverDepartmentID = receiverDepartment.DepartmentID;
        }
    }
}
