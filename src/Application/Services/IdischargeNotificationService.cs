public interface IDischargeNotificationService
{
    Task NotifyPendingDischargeProposalsAsync();
    Task NotifyDischargeApprovalAsync(string dischargeId);
    Task NotifyDischargeRejectionAsync(string proposalId);
}