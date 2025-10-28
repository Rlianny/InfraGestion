public interface INotificationService
{
    Task SendDischargeProposalNotificationAsync(string proposalId);
    Task SendTransferConfirmationNotificationAsync(string transferId);
    Task SendMaintenanceAssignmentNotificationAsync(string maintenanceId);
    Task SendEquipmentReviewAssignmentAsync(string equipmentId, string technicianId);
}