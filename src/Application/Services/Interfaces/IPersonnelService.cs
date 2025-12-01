using Application.DTOs.Personnel;

namespace Application.Services.Interfaces
{
    public interface IPersonnelService
    {
        Task<TechnicianDto> GetTechnicianAsync(int technicianId);
        Task<IEnumerable<TechnicianDto>> GetAllTechniciansAsync();
        Task RateTechnicianPerformanceAsync(RateTechnicianRequest request);
        Task RegisterBonusAsync(BonusRequest request);
        Task<List<BonusDto>> GetTechnicianBonusesAsync(string technicianName);
        Task RegisterPenaltyAsync(PenaltyRequest request);
        Task<List<PenaltyDto>> GetTechnicianPenaltyAsync(string technicianName);
        Task<IEnumerable<RateDto>> GetTechnicianPerformanceHistoryAsync(string technicianName);
    }
}