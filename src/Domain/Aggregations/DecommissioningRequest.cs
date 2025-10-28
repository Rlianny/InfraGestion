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
        public Enums.RequestStatus Status { get; private set; }
        private DecommissioningRequest() { }
        public DecommissioningRequest(int technicianID, int deviceID, int deviceReceiverID, DateTime date)
        {
            ValidateDate(date);
            Date = date;
            TechnicianID = technicianID;
            DeviceID = deviceID;
            DeviceReceiverID = deviceReceiverID;
        }

        private void ValidateDate(DateTime date)
        {
            if (date > DateTime.Now)
                throw new ArgumentException("Rating date cannot be in the future");

        }

        public void Approve()
        {
            if (Status != Enums.RequestStatus.Pending)
            {
                throw new InvalidOperationException("Only pending requests can be approved");
            }
            Status = Enums.RequestStatus.Approved;
        }

        public void Reject()
        {
            if (Status != Enums.RequestStatus.Pending)
            {
                throw new InvalidOperationException("Only pending requests can be rejected");
            }
            Status = Enums.RequestStatus.Rejected;
        }

        public bool IsPending() => Status == Enums.RequestStatus.Pending;
    }
}
