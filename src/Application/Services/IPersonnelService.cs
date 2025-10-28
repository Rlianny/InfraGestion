public interface IPersonnelService
{
    Task<TechnicianDto> GetTechnicianAsync(string technicianId);
    Task<List<TechnicianDto>> GetAllTechniciansAsync();
    Task<PerformanceRatingDto> RateTechnicianPerformanceAsync(RateTechnicianRequest request);
    Task<BonusPenaltyDto> RegisterBonusPenaltyAsync(RegisterBonusPenaltyRequest request);
    Task<List<PerformanceRatingDto>> GetTechnicianPerformanceHistoryAsync(string technicianId);
    Task<List<BonusPenaltyDto>> GetTechnicianBonusesPenaltiesAsync(string technicianId);
}