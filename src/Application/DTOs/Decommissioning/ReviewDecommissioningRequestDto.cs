using System.Runtime.InteropServices;
using Domain.Enums;

namespace Application.DTOs.Decommissioning
{
    public class ReviewDecommissioningRequestDto
    {
        public int DecommissioningRequestId { get; set; }
        public bool IsApproved { get; set; }
        public DateTime date { get; set; }

        // if is approved
        public DecommissioningReason? DecommissioningReason { get; set; } // EOL, IrreparableTechnicalFailure, etc.
        public int? FinalDestination { get; set; }
        public string description { get; set; } = string.Empty;
        public int? ReceiverID { get; set; }
        public int? logisticId { get; set; }
    }
}