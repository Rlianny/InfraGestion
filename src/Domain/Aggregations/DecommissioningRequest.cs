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
        public int DecommissioningRequestID { get; private set; }
        public int TechnicianID { get; private set; }
        public int DeviceID { get; private set; }
        public DateTime Date { get; private set; }
        public int DeviceReceiverID { get; private set; }

        private DecommissioningRequest() { }
        public DecommissioningRequest(DateTime date, int technicianID, int deviceID, int deviceReceiverID)
        {
            Date = date;
            TechnicianID = technicianID;
            DeviceID = deviceID;
            DeviceReceiverID = deviceReceiverID;
        }
    }
}
