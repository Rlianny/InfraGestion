using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Aggregations
{
    public class Mainteinance
    {
        public Mainteinance(Guid technicianID, Guid equipmentID, DateOnly date, double cost, string type)
        {
            TechnicianID = technicianID;
            EquipmentID = equipmentID;
            Date = date;
            Cost = cost;
            Type = type;
        }

        public Guid TechnicianID { get; set; }
        public Guid EquipmentID { get; set; }
        public DateOnly Date { get; set; }
        public double Cost { get; set; }    
        public string Type { get; set; }

        // Navigation properties
        public virtual Technician? Technician { get; set; }
        public virtual Equipment? Equipment { get; set; }
    }
}
