namespace Application.Services.Interfaces
{
    public interface IPersonnelService
    {
        Task<TechnicianDto> GetTechnicianAsync(string technicianId);
        Task<IEnumerable<TechnicianDto>> GetAllTechniciansAsync();
        Task RateTechnicianPerformanceAsync(RateTechnicianRequest request);
        Task RegisterBonusAsync(BonusRequest request);
        Task<List<BonusDto>> GetTechnicianBonusesAsync(int technicianId);
        Task RegisterPenaltyAsync(PenaltyRequest request);
        Task<List<PenaltyDto>> GetTechnicianPenaltyAsync(int technicianId);
        Task<IEnumerable<RateDto>> GetTechnicianPerformanceHistoryAsync(int technicianId);
    }
}