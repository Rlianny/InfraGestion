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
        public Rejection(DateTime decommissioningRequestDate, DateTime rejectionDate, int deviceReceiverID, int technicianID, int deviceID)
        {
            ValidateRequestDate(decommissioningRequestDate);
            ValidateRejectionDate(rejectionDate, decommissioningRequestDate);
            DecommissioningRequestDate = decommissioningRequestDate;
            RejectionDate = rejectionDate;
            DeviceReceiverID = deviceReceiverID;
            TechnicianID = technicianID;
            DeviceID = deviceID;
        }
        private Rejection()
        {
        }
        public int RejectionID { get; private set; }
        public int DeviceReceiverID { get; private set; }
        public int TechnicianID { get; private set; }
        public int DeviceID { get; private set; }
        public DateTime DecommissioningRequestDate { get; private set; }
        public DateTime RejectionDate { get; private set; }
        private void ValidateRequestDate(DateTime date)
        {
            if (date > DateTime.Now)
                throw new ArgumentException("Request date cannot be in the future", nameof(date));
        }

        private void ValidateRejectionDate(DateTime rejectionDate, DateTime requestDate)
        {
            if (rejectionDate > DateTime.Now)
                throw new ArgumentException("Rejection date cannot be in the future", nameof(rejectionDate));

            if (rejectionDate < requestDate)
                throw new ArgumentException("Rejection date cannot be before request date", nameof(rejectionDate));
        }
    }

}
