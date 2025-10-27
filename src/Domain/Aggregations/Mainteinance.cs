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
        public int TechnicianID { get; set; }
        public int DeviceID { get; set; }
        public DateOnly Date { get; set; }
        public double Cost { get; set; }    
        public string Type { get; set; }

        public virtual Technician? Technician { get; set; }
        public virtual Device? Device { get; set; }

        public Mainteinance(Technician technician, Device device, DateOnly date, double cost, string type)
        {
            Technician = technician;
            Device = device;
            TechnicianID = technician.UserID;
            DeviceID = device.DeviceID;
            Date = date;
            Cost = cost;
            Type = type;
        }
    }
}
