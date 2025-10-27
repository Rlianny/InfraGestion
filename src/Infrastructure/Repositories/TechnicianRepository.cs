using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TechnicianRepository : Repository<Technician>, ITechnicianRepository
    {
        public TechnicianRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Technician?> GetTechnicianWithDetailsAsync(int technicianId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(t => t.DepartmentID)
                .FirstOrDefaultAsync(t => t.UserID == technicianId, cancellationToken);
        }

        public async Task<IEnumerable<Technician>> GetTechniciansBySpecialtyAsync(string specialty, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(t => t.Specialty == specialty)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Technician>> GetTechniciansByExperienceAsync(int minYears, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(t => t.YearsOfExperience >= minYears)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Technician>> GetTechniciansByDepartmentAsync(int departmentId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(t => t.DepartmentID == departmentId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Technician>> GetTopPerformingTechniciansAsync(int count, CancellationToken cancellationToken = default)
        {
            // This would typically join with Assessments table and calculate average scores
            // For now, returning technicians ordered by experience
            return await _dbSet
                .OrderByDescending(t => t.YearsOfExperience)
                .Take(count)
                .ToListAsync(cancellationToken);
        }
    }
}
