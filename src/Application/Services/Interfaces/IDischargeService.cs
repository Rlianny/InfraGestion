public interface IDischargeService
{
    Task<DischargeProposalDto> ProposeDischargeAsync(ProposeDischargeRequest request);
    Task<List<DischargeProposalDto>> GetPendingDischargeProposalsAsync();
    Task<DischargeProposalDto> ApproveDischargeAsync(string proposalId, ApproveDischargeRequest request);
    Task<DischargeProposalDto> RejectDischargeAsync(string proposalId, RejectDischargeRequest request);
    Task<DischargeRecordDto> GetDischargeRecordAsync(string dischargeId);
    Task AssignResponsibleDepartmentAsync(string equipmentId, string departmentId);
}