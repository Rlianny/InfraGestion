namespace Application.DTOs.Transfer
{
    public class CreateTransferRequestDto
    {
        public string DeviceName { get; set; } = string.Empty;
        public string DestinationSectionName { get; set; } = string.Empty;
        public string DeviceReceiverUsername { get; set; } = string.Empty;
        public DateTime TransferDate { get; set; }
    }
}