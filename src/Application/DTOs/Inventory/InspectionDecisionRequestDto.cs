namespace Application.DTOs.Inventory;

public class InspectionDecisionRequestDto
{
    public int DeviceId { get; set; }
    public int TechnicianId { get; set; }
    public bool IsApproved { get; set; }
    public string? Reason { get; set; }
}
