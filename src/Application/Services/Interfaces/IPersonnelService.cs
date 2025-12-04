using Application.DTOs.Personnel;
using Application.DTOs.Inventory;

namespace Application.Services.Interfaces
{
    public interface IPersonnelService
    {
        Task<TechnicianDto> GetTechnicianAsync(int technicianId);
        Task<IEnumerable<TechnicianDto>> GetAllTechniciansAsync();
        Task<TechnicianDto> UpdateTechnicianAsync(UpdateTechnicianRequest request);
        Task RateTechnicianPerformanceAsync(RateTechnicianRequest request);
        Task RegisterBonusAsync(BonusRequest request);
        Task<List<BonusDto>> GetTechnicianBonusesAsync(int technicianId);
        Task RegisterPenaltyAsync(PenaltyRequest request);
        Task<List<PenaltyDto>> GetTechnicianPenaltyAsync(int technicianId);
        Task<IEnumerable<RateDto>> GetTechnicianPerformanceHistoryAsync(int technicianId);
        Task<IEnumerable<DeviceDto>> GetTechnicianPendingDevicesAsync(int technicianId);
    }
}