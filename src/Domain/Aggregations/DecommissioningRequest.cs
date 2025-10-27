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
        public int TechnicianID { get; set; }
        public int EquipmentID { get; set; }
        public DateTime Date { get; set; }
        public int EquipmentReceiverID { get; set; }

        // Navigation properties
        public virtual Technician? Technician { get; set; }
        public virtual Device? Equipment { get; set; }
        public virtual EquipmentReceiver? EquipmentReceiver { get; set; }
        public DecommissioningRequest(DateTime date)
        {
            Date = date;
        }
    }
}
