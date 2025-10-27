using Domain.Aggregations;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ReceivingInspectionRequestRepository : Repository<ReceivingInspectionRequest>, IReceivingInspectionRequestRepository
    {
        public ReceivingInspectionRequestRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ReceivingInspectionRequest>> GetReceivingInspectionRequestsByDeviceAsync(int deviceId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(r => r.DeviceID == deviceId)
                .Include(r => r.Administrator)
                .Include(r => r.Technician)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<ReceivingInspectionRequest>> GetReceivingInspectionRequestsByAdministratorAsync(int administratorId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(r => r.AdministratorID == administratorId)
                .Include(r => r.Device)
                .Include(r => r.Technician)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<ReceivingInspectionRequest>> GetReceivingInspectionRequestsByTechnicianAsync(int technicianId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(r => r.TechnicianID == technicianId)
                .Include(r => r.Device)
                .Include(r => r.Administrator)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<ReceivingInspectionRequest>> GetPendingInspectionRequestsAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(r => r.AcceptedDate == null && r.RejectionDate == null)
                .Include(r => r.Device)
                .Include(r => r.Administrator)
                .Include(r => r.Technician)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<ReceivingInspectionRequest>> GetAcceptedInspectionRequestsAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(r => r.AcceptedDate != null)
                .Include(r => r.Device)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<ReceivingInspectionRequest>> GetRejectedInspectionRequestsAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(r => r.RejectionDate != null)
                .Include(r => r.Device)
                .ToListAsync(cancellationToken);
        }
    }
}
