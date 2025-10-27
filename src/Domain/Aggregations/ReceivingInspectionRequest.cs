using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Aggregations
{
    public class ReceivingInspectionRequest
    {
        public ReceivingInspectionRequest(DateTime emissionDate, Device device, Administrator administrator, Technician technician)
        {
            EmissionDate = emissionDate;
            Device = device;
            Administrator = administrator;
            Technician = technician;
        }
        private ReceivingInspectionRequest()
        {
        }

        public int DeviceID { get; private set; }
        public int AdministratorID { get; private set; }
        public int TechnicianID { get; private set; }
        public DateTime EmissionDate { get; private set; }
        public DateTime? AcceptedDate { get; private set; }
        public DateTime? RejectionDate { get; private set; }

        // Navigation properties
        public virtual Device? Device { get; private set; }
        public virtual Administrator? Administrator { get; private set; }
        public virtual Technician? Technician { get; private set; }
    }
}
