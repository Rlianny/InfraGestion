using Domain.Enums;

namespace Application.DTOs.Decommissioning
{
    public class ReviewDecommissioningRequestDto
    {
        public int DecommissioningRequestId { get; set; }
        public bool IsApproved { get; set; }
        // if is approved
        public DecommissioningReason? DecommissioningReason { get; set; } // EOL, IrreparableTechnicalFailure, etc.
        public string? FinalDestination { get; set; } // "Almacén", "Desecho", "Reciclaje", etc.
        public int? ReceiverDepartmentId { get; set; }
    }
}