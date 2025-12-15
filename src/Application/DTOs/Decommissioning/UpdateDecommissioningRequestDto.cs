using Domain.Enums;

namespace Application.DTOs.Decommissioning
{
    public class UpdateDecommissioningRequestDto
    {
        public int DecommissioningRequestId { get; set; }
        public int TechnicianId { get; set; }
        public int DeviceId { get; set; }
        public DateTime EmissionDate { get; set; }
        public DateTime? AnswerDate { get; set; }
        public RequestStatus Status { get; set; }
        public DecommissioningReason Reason { get; set; }
        public int? DeviceReceiverId { get; set; }
        public bool? IsApproved { get; set; }
        public int? FinalDestinationDepartmentId { get; set; }
        public int? LogisticId { get; set; }
        public string Description { get; set; } = "";
    }
}