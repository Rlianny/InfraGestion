namespace Application.DTOs.Transfer
{
    public class TransferSummaryDto
    {
        public int TransferId { get; set; }
        public DateTime TransferDate { get; set; }
        public string SourceSectionName { get; set; } = string.Empty;
        public string DestinationSectionName { get; set; } = string.Empty;
        public string InitiatedByUserName { get; set; } = string.Empty;
        public TransferStatus Status { get; set; } = TransferStatus.Pending;
    }
}