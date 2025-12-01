using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TechnicianRepository : Repository<User>, ITechnicianRepository
    {
        
        private const int TechnicianRoleId = (int)RoleEnum.Technician;

        public TechnicianRepository(ApplicationDbContext context) : base(context)
        {
        }
        public override async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(t => t.RoleId == TechnicianRoleId)
                .ToListAsync(cancellationToken);
        }

        public async Task<User?> GetTechnicianWithDetailsAsync(int technicianId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(t => t.RoleId == TechnicianRoleId)
                .Include(t => t.Department)
                .FirstOrDefaultAsync(t => t.UserId == technicianId, cancellationToken);
        }

        public async Task<IEnumerable<User>> GetTechniciansBySpecialtyAsync(string specialty, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(t => t.Specialty == specialty && t.RoleId == TechnicianRoleId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<User>> GetTechniciansByExperienceAsync(int minYears, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(t => t.RoleId == TechnicianRoleId && t.YearsOfExperience >= minYears)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<User>> GetTechniciansByDepartmentAsync(int departmentId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(t => t.RoleId == TechnicianRoleId && t.DepartmentId == departmentId)
                .ToListAsync(cancellationToken);
        }

        public async Task<User?> GetByNameAsync(string technicianName, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(t => t.RoleId == TechnicianRoleId)
                .FirstOrDefaultAsync(t => t.FullName == technicianName, cancellationToken);
        }

        public async Task<IEnumerable<User>> GetTopPerformingTechniciansAsync(int count, CancellationToken cancellationToken = default)
        {
            // This would typically join with Assessments table and calculate average scores
            // For now, returning technicians ordered by experience
            return await _dbSet
                .Where(t => t.RoleId == TechnicianRoleId)
                .OrderByDescending(t => t.YearsOfExperience)
                .Take(count)
                .ToListAsync(cancellationToken);
        }
    }
}
