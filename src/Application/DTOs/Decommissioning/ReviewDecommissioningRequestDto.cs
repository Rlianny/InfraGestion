namespace Application.DTOs.Decommissioning
{
    public class ReviewDecommissioningRequestDto
    {
        public int DecommissioningRequestId { get; set; }
        public bool IsApproved { get; set; }
        public string? RejectionReason { get; set; } // if is rejected
        
        // if is approved
        public string? FinalDestination { get; set; } // "Almacén", "Desecho", "Reciclaje", etc.
        public int? ReceiverDepartmentId { get; set; }
        public string? ReceiverName { get; set; }
    }
}