using System.Security.Cryptography.X509Certificates;
using Domain.Enums;

namespace Domain.Aggregations
{
    public class DecommissioningRequest
    {
        public int DecommissioningRequestId { get; private set; }
        public int TechnicianId { get; private set; }
        public int DeviceId { get; private set; }
        public DateTime EmissionDate { get; private set; }
        public DateTime? AnswerDate { get; private set; }
        public Enums.RequestStatus Status { get; private set; }
        public DecommissioningReason Reason { get; private set; }
        public int? DeviceReceiverId { get; private set; }
        public bool? IsApproved { get; private set; }
        public int? FinalDestinationDepartmentID { get; private set; }
        public int? logisticId { get; private set; }
        public string description { get; private set; } = "";
        public DecommissioningRequest(
            int technicianId,
            int deviceId,
            DateTime emissionDate,
            DecommissioningReason reason
        )
        {
            ValidateDate(emissionDate);
            Status = Enums.RequestStatus.Pending;
            EmissionDate = emissionDate;
            TechnicianId = technicianId;
            DeviceId = deviceId;
            Reason = reason;
            DeviceReceiverId = null;
        }

        private void ValidateDate(DateTime date)
        {
            if (date > DateTime.Now)
                throw new ArgumentException("Rating date cannot be in the future");

        }

        public void Approve(int logisticId, int deviceReceiverId, int FinalDestination, string description, DateTime answerDate, DecommissioningReason reason)
        {
            if (Status != Enums.RequestStatus.Pending)
            {
                throw new InvalidOperationException("Only pending requests can be approved");
            }
            Status = Enums.RequestStatus.Approved;
            IsApproved = true;
            this.logisticId = logisticId;
            this.DeviceReceiverId = deviceReceiverId;
            this.AnswerDate = answerDate;
            this.FinalDestinationDepartmentID = FinalDestination;
            this.description = description;
            this.Reason = reason;
        }

        public void Reject(int logisticId, string description, DateTime answerDate)
        {
            if (Status != Enums.RequestStatus.Pending)
            {
                throw new InvalidOperationException("Only pending requests can be rejected");
            }
            Status = Enums.RequestStatus.Rejected;
            IsApproved = false;
            this.logisticId = logisticId;
            this.description = description;
            this.AnswerDate = answerDate;

        }

        public bool IsPending() => Status == Enums.RequestStatus.Pending;

        private DecommissioningRequest()
        {
        }
    }
}
