

namespace Domain.Aggregations
{
    public class DecommissioningRequest
    {
        public int DecommissioningRequestId { get; private set; }
        public int TechnicianId { get; private set; }
        public int DeviceId { get; private set; }
        public DateTime Date { get; private set; }
        public int DeviceReceiverId { get; private set; }
        public Enums.RequestStatus Status { get; private set; }
        public string Reason { get; private set; } = string.Empty;
        private DecommissioningRequest() { }
        public DecommissioningRequest(
            int technicianId,
            int deviceId,
            int deviceReceiverId,
            DateTime date,
            string reason
        )
        {
            ValidateDate(date);
            ValidateReason(reason);
            Status = Enums.RequestStatus.Pending;
            Date = date;
            TechnicianId = technicianId;
            DeviceId = deviceId;
            DeviceReceiverId = deviceReceiverId;
            Reason = reason;
        }

        private void ValidateDate(DateTime date)
        {
            if (date > DateTime.Now)
                throw new ArgumentException("Rating date cannot be in the future");

        }

        private void ValidateReason(string reason)
        {
            if (string.IsNullOrWhiteSpace(reason))
                throw new ArgumentException("Reason is required for a decommissioning request");
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
