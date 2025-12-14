using Domain.Enums;
namespace Application.DTOs
{
    public class UpdateDecommissioningRequestDto
    {
        public int DecommissioningRequestId { get; private set; }
        public int TechnicianId { get; private set; }
        public int DeviceId { get; private set; }
        public DateTime EmissionDate { get; private set; }
        public DateTime? AnswerDate { get; private set; }
        public RequestStatus Status { get; private set; }
        public DecommissioningReason Reason { get; private set; }
        public int? DeviceReceiverId { get; private set; }
        public bool? IsApproved { get; private set; }
        public int? FinalDestinationDepartmentID { get; private set; }
        public int? logisticId { get; private set; }
        public string description { get; private set; } = "";
    }
}