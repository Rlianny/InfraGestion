using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class EquipmentReceiverRepository : Repository<EquipmentReceiver>, IEquipmentReceiverRepository
    {
        public EquipmentReceiverRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<EquipmentReceiver?> GetEquipmentReceiverWithDetailsAsync(int equipmentReceiverId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(er => er.Department)
                .FirstOrDefaultAsync(er => er.UserID == equipmentReceiverId, cancellationToken);
        }

        public async Task<IEnumerable<EquipmentReceiver>> GetEquipmentReceiversByDepartmentAsync(int departmentId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(er => er.DepartmentID == departmentId)
                .ToListAsync(cancellationToken);
        }
    }
}
