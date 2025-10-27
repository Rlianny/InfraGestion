using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Aggregations
{
    public class Rejection
    {
        public Rejection(DeviceReceiver deviceReceiver, DecommissioningRequest decommissioningRequest, DateTime rejectionDate)
        {
            RejectionDate = rejectionDate;
            DecommissioningRequestDate = decommissioningRequest.Date;
            DecommissioningRequest = decommissioningRequest;
            RejectionDate = rejectionDate;
            DeviceReceiver = deviceReceiver;
            Technician = decommissioningRequest.Technician;
            Device = decommissioningRequest.Device;

            DeviceReceiverID = DeviceReceiver.UserID;
            TechnicianID = Technician.UserID;
            DeviceID = Device.DeviceID;
            

        }
        private Rejection() { }

        public int RejectionID {get; private set;}
        public int DeviceReceiverID { get; private set; }
        public int TechnicianID { get; private set; }
        public int DeviceID { get; private set; }
        public DateTime DecommissioningRequestDate { get; private set; }
        public DateTime RejectionDate { get; private set; }

        // Navigation properties
        public virtual Entities.DeviceReceiver? DeviceReceiver { get; private set; }
        public virtual Entities.Technician? Technician { get; private set; }
        public virtual Entities.Device? Device { get; private set; }
        public virtual DecommissioningRequest? DecommissioningRequest { get; private set; }
    }
}
