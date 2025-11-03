using Application.DTOs.Decommissioning;
using Domain.Aggregations;

public interface IDecommissioningService
{
    Task ProposeDecommissionAsync(CreateDecommissioningRequestDto createDecommissioning);
    Task<IEnumerable<DecommissioningRequestDto>> GetPendingDecommissionProposalsAsync();
    Task ApproveDecommissionAsync(DecommissioningDto decommissioningInfo);
    Task RejectDecommissionAsync(ReviewDecommissioningRequestDto rejectionDto);
    Task<DecommissioningRequestDto> GetDecommissionRecordAsync(int proposalId);
    Task AssignResponsibleDepartmentAsync(int deviceId, int departmentId);
}