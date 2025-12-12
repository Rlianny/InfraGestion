namespace Application.DTOs.InspectionDTOs
{
    public class InspectionReviewRequestDto
    {
        public int InspectionRequestId { get; set; }
        public bool IsApproved { get; set; }
        public int? TargetDepartmentId { get; set; } // if is approved
        public string? RejectionReason { get; set; } // if is rejected
    }
}