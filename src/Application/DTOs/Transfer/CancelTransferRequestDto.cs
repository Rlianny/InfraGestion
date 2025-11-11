namespace Application.DTOs.Transfer
{
    public class CancelTransferRequestDto
    {
        public int TransferId { get; set; }
        public string CancellationReason { get; set; } = string.Empty;
    }
}