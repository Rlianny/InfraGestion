using Application.DTOs.Decommissioning;
using Domain.Aggregations;

namespace Application.Services.Interfaces
{
    public interface IdecommissioningService
    {
        Task ProposeDecommissionAsync(CreateDecommissioningRequestDto createDecommissioning);
        Task<IEnumerable<DecommissioningRequestDto>> GetPendingDecommissionProposalsAsync();
        Task ApproveDecommissionAsync(DecommissioningDto decommissioningInfo);
        Task RejectDecommissionAsync(ReviewDecommissioningRequestDto rejectionDto);
        Task<DecommissioningRequestDto> GetDecommissionRecordAsync(int proposalId);
        Task AssignResponsibleDepartmentAsync(int deviceId, int departmentId);
    }
}