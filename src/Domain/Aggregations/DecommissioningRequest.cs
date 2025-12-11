

using Domain.Enums;

namespace Domain.Aggregations
{
    public class DecommissioningRequest
    {
        public int DecommissioningRequestId { get; private set; }
        public int TechnicianId { get; private set; }
        public int DeviceId { get; private set; }
        public DateTime Date { get; private set; }
        public Enums.RequestStatus Status { get; private set; }
        public DecommissioningReason Reason { get; private set; }
        public bool IsApproved { get; private set; }
        private DecommissioningRequest() { }
        public DecommissioningRequest(
            int technicianId,
            int deviceId,
            DateTime date,
            DecommissioningReason reason
        )
        {
            ValidateDate(date);
            Status = Enums.RequestStatus.Pending;
            Date = date;
            TechnicianId = technicianId;
            DeviceId = deviceId;
            Reason = reason;
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
