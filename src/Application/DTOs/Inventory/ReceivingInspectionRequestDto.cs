using Domain.Enums;

namespace Application.DTOs.Inventory;

public class ReceivingInspectionRequestDto
{
    public int RequestId { get; set; }
    public DateTime RequestDate { get; set; }
    public int DeviceId { get; set; }
    public int UserId { get; set; }
    public int TechnicianId { get; set; }
    public InspectionRequestStatus Status { get; set; }
    public string RejectReason { get; set; }

    public ReceivingInspectionRequestDto(int requestId, DateTime requestDate, int deviceId, int userId, int technicianId, InspectionRequestStatus status, string rejectReason)
    {
        RequestId = requestId;
        RequestDate = requestDate;
        DeviceId = deviceId;
        UserId = userId;
        TechnicianId = technicianId;
        Status = status;
        RejectReason = rejectReason;
    }
}