using Domain.Enums;

namespace Application.DTOs.InspectionDTOs;

public class InspectionDecisionRequestDto
{
    public int DeviceId { get; set; }
    public int TechnicianId { get; set; }
    public bool IsApproved { get; set; }
    public DecommissioningReason? Reason { get; set; }
}
