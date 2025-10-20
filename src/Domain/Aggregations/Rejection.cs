using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Aggregations
{
    public class Rejection
    {
        public Rejection(Guid equipmentReceiverID, Guid technicianID, Guid equipmentID, DateTime decommissioningRequestDate, DateTime rejectionDate)
        {
            EquipmentReceiverID = equipmentReceiverID;
            TechnicianID = technicianID;
            EquipmentID = equipmentID;
            DecommissioningRequestDate = decommissioningRequestDate;
            RejectionDate = rejectionDate;
        }

        public Guid EquipmentReceiverID { get; set; }
        public Guid TechnicianID { get; set; }
        public Guid EquipmentID { get; set; }
        public DateTime DecommissioningRequestDate { get; set; }
        public DateTime RejectionDate { get; set; }

        // Navigation properties
        public virtual Entities.EquipmentReceiver? EquipmentReceiver { get; set; }
        public virtual Entities.Technician? Technician { get; set; }
        public virtual Entities.Equipment? Equipment { get; set; }
    }
}
