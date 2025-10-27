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
        public int DeviceID { get; set; }
        public DateTime Date { get; set; }
        public int EquipmentReceiverID { get; set; }

        // Navigation properties
        public virtual Technician? Technician { get; set; }
        public virtual Device? Device { get; set; }
        public virtual DeviceReceiver? DeviceReceiver { get; set; }
        private DecommissioningRequest() {}
        public DecommissioningRequest(Technician technician, Device device, DeviceReceiver deviceReceiver, DateTime date)
        {
            Technician = technician;
            Device = device;
            DeviceReceiver = deviceReceiver;
            Date = date;
            TechnicianID = technician.UserID;
            DeviceID = device.DeviceID;
            EquipmentReceiverID = deviceReceiver.UserID;
        }
    }
}
