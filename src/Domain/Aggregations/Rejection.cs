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
        public Rejection(DateTime decommissioningRequestDate, DateTime rejectionDate, int deviceReceiverId, int technicianId, int deviceId)
        {
            ValidateRequestDate(decommissioningRequestDate);
            ValidateRejectionDate(rejectionDate, decommissioningRequestDate);
            DecommissioningRequestDate = decommissioningRequestDate;
            RejectionDate = rejectionDate;
            DeviceReceiverId = deviceReceiverId;
            TechnicianId = technicianId;
            DeviceId = deviceId;
        }
        private Rejection()
        {
        }
        public int RejectionId { get; private set; }
        public int DeviceReceiverId { get; private set; }
        public int TechnicianId { get; private set; }
        public int DeviceId { get; private set; }
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
