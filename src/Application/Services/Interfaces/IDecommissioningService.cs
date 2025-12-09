using Application.DTOs.Decommissioning;
using Domain.Enums;

namespace Application.Services.Interfaces
{
    public interface IDecommissioningService
    {
        #region DecommissioningRequest (Solicitudes de baja)

        Task CreateDecommissioningRequestAsync(CreateDecommissioningRequestDto request);
        Task<IEnumerable<DecommissioningRequestDto>> GetPendingRequestsAsync();
        Task<IEnumerable<DecommissioningRequestDto>> GetAllRequestsAsync();
        Task<DecommissioningRequestDto> GetRequestByIdAsync(int requestId);
        Task<IEnumerable<DecommissioningRequestDto>> GetRequestsByDeviceIdAsync(int deviceId);

        // Review request (Administrator/Director)
        Task ReviewDecommissioningRequestAsync(ReviewDecommissioningRequestDto review);

        #endregion

        #region Decommissioning (Bajas finalizadas)

        // Get finalized decommissionings
        Task<IEnumerable<DecommissioningDto>> GetAllDecommissioningsAsync();
        Task<DecommissioningDto> GetDecommissioningByIdAsync(int decommissioningId);
        Task<DecommissioningDto?> GetDecommissioningByDeviceIdAsync(int deviceId); // There can only be ONE decommissioning per device
        Task<IEnumerable<DecommissioningDto>> GetDecommissioningsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<DecommissioningDto>> GetDecommissioningsByDepartmentAsync(int departmentId);
        Task<IEnumerable<DecommissioningDto>> GetDecommissioningsByReasonAsync(DecommissioningReason reason);

        #endregion
    }
}