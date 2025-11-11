namespace Application.DTOs.Transfer
{
    public class CreateTransferRequestDto
    {
        public int DeviceId { get; set; }
        public int SourceSectionId { get; set; }
        public int DestinationSectionId { get; set; }
        public int DeviceReceiverId { get; set; }
        public DateTime TransferDate { get; set; }
    }
}