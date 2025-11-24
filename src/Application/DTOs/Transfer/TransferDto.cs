using Domain.Enums;
namespace Application.DTOs.Transfer
{
    public class TransferDto
    {
        public TransferDto(int transferId, int deviceId, string deviceName, DateOnly transferDate, int sourceSectionId, string sourceSectionName, int destinationSectionId, string destinationSectionName, int deviceReceiverId, string deviceReceiverName, TransferStatus status)
        {
            TransferId = transferId;
            DeviceId = deviceId;
            DeviceName = deviceName;
            TransferDate = transferDate;
            SourceSectionId = sourceSectionId;
            SourceSectionName = sourceSectionName;
            DestinationSectionId = destinationSectionId;
            DestinationSectionName = destinationSectionName;
            DeviceReceiverId = deviceReceiverId;
            DeviceReceiverName = deviceReceiverName;
            Status = status;
        }

        public int TransferId { get; set; }
        public int DeviceId { get; set; }
        public string DeviceName { get; set; } = string.Empty;
        public DateOnly TransferDate { get; set; }
        public int SourceSectionId { get; set; }
        public string SourceSectionName { get; set; } = string.Empty;
        public int DestinationSectionId { get; set; }
        public string DestinationSectionName { get; set; } = string.Empty;
        public int DeviceReceiverId { get; set; }
        public string DeviceReceiverName { get; set; } = string.Empty;
        public TransferStatus Status { get; set; } = TransferStatus.Pending; // Pending, InTransit, Completed, Cancelled
    }
}