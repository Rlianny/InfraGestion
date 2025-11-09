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
        public ReceivingInspectionRequest(DateTime emissionDate, int deviceId, int administratorId, int technicianId)
        {
            ValidateDate(emissionDate);
            Status = Enums.InspectionRequestStatus.Pending;
            EmissionDate = emissionDate;
            DeviceId = deviceId;
            AdministratorId = administratorId;
            TechnicianId = technicianId;
        }
        private ReceivingInspectionRequest() { }

        public int ReceivingInspectionRequestId { get; private set; }
        public int DeviceId { get; private set; }
        public int AdministratorId { get; private set; }
        public int TechnicianId { get; private set; }
        public DateTime EmissionDate { get; private set; }
        public DateTime? AcceptedDate { get; private set; }
        public DateTime? RejectionDate { get; private set; }
        public Enums.InspectionRequestStatus Status { get; private set; }
        public string RejectReason { get; private set; }
        private void ValidateDate(DateTime date)
        {
            if (date > DateTime.Now)
            {
                throw new ArgumentException();
            }
        }

        public void Accept()
        {
            if (Status != Enums.InspectionRequestStatus.Pending)
                throw new InvalidOperationException("Only pending inspections can be accepted");

            if (AcceptedDate.HasValue || RejectionDate.HasValue)
                throw new InvalidOperationException("Inspection already processed");

            AcceptedDate = DateTime.Now;
            Status = Enums.InspectionRequestStatus.Accepted;
        }

        public void Reject(string reason)
        {
            if (Status != Enums.InspectionRequestStatus.Pending)
                throw new InvalidOperationException("Only pending inspections can be rejected");

            if (AcceptedDate.HasValue || RejectionDate.HasValue)
                throw new InvalidOperationException("Inspection already processed");

            RejectionDate = DateTime.Now;
            Status = Enums.InspectionRequestStatus.Rejected;
            RejectReason = reason;
        }

        public bool IsPending() => Status == Enums.InspectionRequestStatus.Pending;

        public bool IsAccepted() => Status == Enums.InspectionRequestStatus.Accepted;

        public bool IsRejected() => Status == Enums.InspectionRequestStatus.Rejected;
    }
}