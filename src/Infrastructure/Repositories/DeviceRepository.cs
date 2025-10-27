using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class DeviceRepository : Repository<Device>, IDeviceRepository
    {
        public DeviceRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Device?> GetDeviceWithDepartmentAsync(int deviceId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(d => d.Department)
                .FirstOrDefaultAsync(d => d.DeviceID == deviceId, cancellationToken);
        }

        public async Task<IEnumerable<Device>> GetDevicesByDepartmentAsync(int departmentId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(d => d.DepartmentID == departmentId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Device>> GetDevicesByTypeAsync(DeviceType type, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(d => d.Type == type)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Device>> GetDevicesByOperationalStateAsync(OperationalState state, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(d => d.OperationalState == state)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Device>> GetDevicesAcquiredBetweenAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(d => d.AcquisitionDate >= startDate && d.AcquisitionDate <= endDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> CountDevicesByStateAsync(OperationalState state, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .CountAsync(d => d.OperationalState == state, cancellationToken);
        }

        public async Task<IEnumerable<Device>> SearchDevicesByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(d => d.Name.Contains(name))
                .ToListAsync(cancellationToken);
        }
    }
}
