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

        public async Task<ReceivingInspectionRequest> GetReceivingInspectionRequestsByDeviceAsync(int deviceId, CancellationToken cancellationToken = default)
        {
            return (await _dbSet
                .Where(r => r.DeviceID == deviceId)
                .Include(r => r.AdministratorID)
                .Include(r => r.TechnicianID)
                .ToListAsync(cancellationToken))[0];
        }

        public async Task<IEnumerable<ReceivingInspectionRequest>> GetReceivingInspectionRequestsByAdministratorAsync(int administratorId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(r => r.AdministratorID == administratorId)
                .Include(r => r.DeviceID)
                .Include(r => r.TechnicianID)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<ReceivingInspectionRequest>> GetReceivingInspectionRequestsByTechnicianAsync(int technicianId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(r => r.TechnicianID == technicianId)
                .Include(r => r.DeviceID)
                .Include(r => r.AdministratorID)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<ReceivingInspectionRequest>> GetPendingInspectionRequestsAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(r => r.AcceptedDate == null && r.RejectionDate == null)
                .Include(r => r.DeviceID)
                .Include(r => r.AdministratorID)
                .Include(r => r.TechnicianID)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<ReceivingInspectionRequest>> GetAcceptedInspectionRequestsAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(r => r.AcceptedDate != null)
                .Include(r => r.DeviceID)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<ReceivingInspectionRequest>> GetRejectedInspectionRequestsAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(r => r.RejectionDate != null)
                .Include(r => r.DeviceID)
                .ToListAsync(cancellationToken);
        }
    }
}
