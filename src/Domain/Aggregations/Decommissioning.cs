using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Aggregations
{
    public class Decommissioning
    {
        public Decommissioning(Guid decommissioningRequestID, Guid equipmentReceiverID, Guid equipmentID, DateTime decommissioningDate, DateTime requestDate, string reason, string finalDestination)
        {
            DecommissioningRequestID = decommissioningRequestID;
            EquipmentReceiverID = equipmentReceiverID;
            EquipmentID = equipmentID;
            DecommissioningDate = decommissioningDate;
            RequestDate = requestDate;
            Reason = reason;
            FinalDestination = finalDestination;
        }

        public Guid DecommissioningRequestID { get; set; }  
        public Guid EquipmentReceiverID { get; set; }
        public Guid EquipmentID { get; set; }
        public DateTime DecommissioningDate { get; set; }
        public DateTime RequestDate { get; set; }
        public Guid DepartmentID { get; set; }
        public string Reason { get; set; }
        public string FinalDestination { get; set; }

        // Navigation properties
        public virtual EquipmentReceiver? EquipmentReceiver { get; set; }
        public virtual Equipment? Equipment { get; set; }
        public virtual Department? Department { get; set; }
    }
}
