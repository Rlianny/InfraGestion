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
        public int DeviceReceiverID { get; set; }
        public int DecommissioningRequestID { get; set; }  
        public int EquipmentID { get; set; }
        
        public DateTime DecommissioningDate
        {
            get { return decommissioningDate; }
            private set { }
        }
        private DateTime decommissioningDate;

        public DecommissioningReason Reason
        {
            get { return reason; }
            private set { }
        }
        private DecommissioningReason reason;

        public string FinalDestination
        {
            get { return finalDestination; }
            private set { }
        }
        private string finalDestination;
        public int ReceiverDepartment { get; set; }
        
        public virtual DeviceReceiver? DeviceReceiver { get; set; }
        public virtual Device? Equipment { get; set; }
        public virtual DecommissioningRequest? DecommissioningRequest { get; set; }
        public virtual Department? Department { get; set; }
        public Decommissioning(Device device, DecommissioningRequest decommissioningRequest,DeviceReceiver deviceReceiver, DateTime decommissioningDate, DecommissioningReason reason, string finalDestination)
        {
            Equipment = device;
            DeviceReceiver = deviceReceiver;
            DecommissioningRequest = decommissioningRequest;
            this.decommissioningDate = decommissioningDate;
            this.reason = reason;
            this.finalDestination = finalDestination;
        }
    }
}
