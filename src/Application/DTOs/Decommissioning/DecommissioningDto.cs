namespace Application.DTOs.Decommissioning
{
    public class DecommissioningDto
    {
        public int DecommissioningId { get; set; }
        public int DeviceId { get; set; }
        public string DeviceName { get; set; } = string.Empty;
        public DateTime DecommissioningDate { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string FinalDestination { get; set; } = string.Empty;
        public int ReceiverDepartmentId { get; set; }
        public string ReceiverDepartmentName { get; set; } = string.Empty;
        public int DeviceReceiverId { get; set; }
        public string DeviceReceiverName { get; set; } = string.Empty;
    }
}