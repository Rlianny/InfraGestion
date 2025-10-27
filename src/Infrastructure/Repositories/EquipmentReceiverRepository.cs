using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class DeviceReceiverRepository : Repository<DeviceReceiver>, IDeviceReceiverRepository
    {
        public DeviceReceiverRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<DeviceReceiver?> GetDeviceReceiverWithDetailsAsync(int DeviceReceiverId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(er => er.DepartmentID)
                .FirstOrDefaultAsync(er => er.UserID == DeviceReceiverId, cancellationToken);
        }

        public async Task<IEnumerable<DeviceReceiver>> GetDeviceReceiversByDepartmentAsync(int departmentId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(er => er.DepartmentID == departmentId)
                .ToListAsync(cancellationToken);
        }
    }
}
