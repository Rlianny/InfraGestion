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
        public ReceivingInspectionRequest(DateTime emissionDate, int deviceID, int administratorID, int technicianID)
        {
            EmissionDate = emissionDate;
            DeviceID = deviceID;
            AdministratorID = administratorID;
            TechnicianID = technicianID;
        }
        private ReceivingInspectionRequest()
        {
        }
        public int ReceivingInspectionRequestID { get; private set; }
        public int DeviceID { get; private set; }
        public int AdministratorID { get; private set; }
        public int TechnicianID { get; private set; }
        public DateTime EmissionDate { get; private set; }
        public DateTime? AcceptedDate { get; private set; }
        public DateTime? RejectionDate { get; private set; }

    }
}