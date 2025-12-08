namespace Application.DTOs.Decommissioning
{
    public class CreateDecommissioningRequestDto
    {
        public int DeviceId { get; set; }
        public int DeviceReceiverId { get; set; }
        public int TechnicianId { get; set; }
        public string Reason { get; set; } = string.Empty;
        public DateTime RequestDate { get; set; }
        
    }
}