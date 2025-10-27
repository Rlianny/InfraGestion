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
        public int DecommissioningRequestID { get; set; }  
        public int EquipmentReceiverID { get; set; }
        public int EquipmentID { get; set; }
        
        public DateTime DecommissioningDate
        {
            get { return decommissioningDate; }
            private set { }
        }
        private DateTime decommissioningDate;

        public DateTime RequestDate
        {
            get { return requestDate; }
            private set { }
        }
        private DateTime requestDate;

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

        public virtual EquipmentReceiver? EquipmentReceiver { get; set; }
        public virtual Equipment? Equipment { get; set; }
        public virtual Department? Department { get; set; }
        public Decommissioning(DateTime decommissioningDate, DateTime requestDate, DecommissioningReason reason, string finalDestination)
        {
            this.decommissioningDate = decommissioningDate;
            this.requestDate = requestDate;
            this.reason = reason;
            this.finalDestination = finalDestination;
        }
    }
}
