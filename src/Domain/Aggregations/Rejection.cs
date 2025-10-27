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
        public Rejection(DateTime decommissioningRequestDate, DateTime rejectionDate, DeviceReceiver deviceReceiver, Device device, Technician technician)
        {
            DecommissioningRequestDate = decommissioningRequestDate;
            RejectionDate = rejectionDate;
            DeviceReceiver = deviceReceiver;
            Technician = technician;
            Device = device;

        }
        private Rejection()
        {
        }

        public int DeviceReceiverID { get; private set; }
        public int TechnicianID { get; private set; }
        public int DeviceID { get; private set; }
        public DateTime DecommissioningRequestDate { get; private set; }
        public DateTime RejectionDate { get; private set; }

        // Navigation properties
        public virtual Entities.DeviceReceiver? DeviceReceiver { get; private set; }
        public virtual Entities.Technician? Technician { get; private set; }
        public virtual Entities.Device? Device { get; private set; }
    }
}
