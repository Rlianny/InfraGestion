using Domain.Enums;
namespace Application.DTOs.Transfer
{
    public class TransferDto
    {
        public int TransferId { get; set; }
        public int DeviceId { get; set; }
        public string DeviceName { get; set; } = string.Empty;
        public DateTime TransferDate { get; set; }
        public int SourceSectionId { get; set; }
        public string SourceSectionName { get; set; } = string.Empty;
        public int DestinationSectionId { get; set; }
        public string DestinationSectionName { get; set; } = string.Empty;
        public int InitiatedByUserId { get; set; }
        public string InitiatedByUserName { get; set; } = string.Empty;
        public int DeviceReceiverId { get; set; }
        public string DeviceReceiverName { get; set; } = string.Empty;
        public TransferStatus Status { get; set; } = TransferStatus.Pending; // Pending, InTransit, Completed, Cancelled
        public DateTime? CompletedDate { get; set; }
    }
}