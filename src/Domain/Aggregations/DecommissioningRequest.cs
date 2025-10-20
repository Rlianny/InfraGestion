using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Aggregations
{
    public class DecommissioningRequest
    {
        public DecommissioningRequest(Guid technicianID, Guid equipmentID, DateTime date)
        {
            TechnicianID = technicianID;
            EquipmentID = equipmentID;
            Date = date;
        }

        public Guid TechnicianID { get; set; }
        public Guid EquipmentID { get; set; }
        public DateTime Date { get; set; }
        public Guid EquipmentReceiverID { get; set; }

        // Navigation properties
        public virtual Technician? Technician { get; set; }
        public virtual Equipment? Equipment { get; set; }
        public virtual EquipmentReceiver? EquipmentReceiver { get; set; }
    }
}
