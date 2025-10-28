using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Aggregations
{
    public class Transfer
    {
        public Transfer(DateOnly dateTime, int deviceID, int sourceSectionID, int destinationSectionID, int deviceReceiverID)
        {
            ValidateTransferDate(Date);
            Status = Enums.TransferStatus.Pending;
            Date = dateTime;
            DeviceID = deviceID;
            SourceSectionID = sourceSectionID;
            DestinationSectionID = destinationSectionID;
            DeviceReceiverID = deviceReceiverID;
        }
        private Transfer()
        {
            Status = Enums.TransferStatus.Pending;
        }

        public int DeviceID { get; private set; }
        public DateOnly Date { get; private set; }
        public int SourceSectionID { get; private set; }
        public int DestinationSectionID { get; private set; }
        public int TransferID { get; private set; }
        public int DeviceReceiverID { get; private set; }
        public Enums.TransferStatus Status { get; private set; }

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
