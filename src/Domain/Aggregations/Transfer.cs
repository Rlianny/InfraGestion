using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Aggregations
{
    public class Transfer:SoftDeleteBase
    {
        public Transfer(DateOnly dateTime, int deviceId, int sourceSectionId, int destinationSectionId, int deviceReceiverId)
        {
            ValidateTransferDate(Date);
            Status = Enums.TransferStatus.Pending;
            Date = dateTime;
            DeviceId = deviceId;
            SourceSectionId = sourceSectionId;
            DestinationSectionId = destinationSectionId;
            DeviceReceiverId = deviceReceiverId;
        }
        private Transfer()
        {
            Status = Enums.TransferStatus.Pending;
        }

        public int DeviceId { get; private set; }
        public DateOnly Date { get; private set; }
        public int SourceSectionId { get; private set; }
        public int DestinationSectionId { get; private set; }
        public int TransferId { get; private set; }
        public int DeviceReceiverId { get; private set; }
        public Enums.TransferStatus Status { get; private set; }
        public override bool IsDisabled { get; set; }

        private void ValidateTransferDate(DateOnly date)
        {
            if (date > DateOnly.FromDateTime(DateTime.Now.AddDays(30)))
                throw new ArgumentException("Transfer date cannot be more than 30 days in the future", nameof(date));
        }

        public void ConfirmReceipt()
        {
            if (Status != Enums.TransferStatus.InTransit)
                throw new InvalidOperationException("Transfer must be in transit to confirm receipt");

            Status = Enums.TransferStatus.Completed;
        }

        public void StartTransit()
        {
            if (Status != Enums.TransferStatus.Pending)
                throw new InvalidOperationException("Only pending transfers can be started");

            Status = Enums.TransferStatus.InTransit;
        }

        public void Cancel()
        {
            if (Status == Enums.TransferStatus.Completed)
                throw new InvalidOperationException("Cannot cancel completed transfer");

            Status = Enums.TransferStatus.Cancelled;
        }

        public bool IsCompleted() => Status == Enums.TransferStatus.Completed;
        public bool IsPending() => Status == Enums.TransferStatus.Pending;

    }
}
