using Domain.Aggregations;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ReceivingInspectionRequestRepository
        : Repository<ReceivingInspectionRequest>,
            IReceivingInspectionRequestRepository
    {
        public ReceivingInspectionRequestRepository(ApplicationDbContext context)
            : base(context) { }

        public async Task<ReceivingInspectionRequest> GetReceivingInspectionRequestsByDeviceAsync(
            int deviceId,
            CancellationToken cancellationToken = default
        )
        {
            return (
                await _dbSet
                    .Where(r => r.DeviceId == deviceId)
                    .ToListAsync(cancellationToken)
            )[0];
        }

        public async Task<
            IEnumerable<ReceivingInspectionRequest>
        > GetInspectionRequestsByAdminAsync(
            int administratorId,
            CancellationToken cancellationToken = default
        )
        {
            return await _dbSet
                .Where(r => r.AdministratorId == administratorId)
                .ToListAsync(cancellationToken);
        }

        public async Task<
            IEnumerable<ReceivingInspectionRequest>
        > GetReceivingInspectionRequestsByTechnicianAsync(
            int technicianId,
            CancellationToken cancellationToken = default
        )
        {
            return await _dbSet
                .Where(r => r.TechnicianId == technicianId)
                .ToListAsync(cancellationToken);
        }

        public async Task<
            IEnumerable<ReceivingInspectionRequest>
        > GetPendingInspectionRequestsAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(r => r.AcceptedDate == null && r.RejectionDate == null)
                .ToListAsync(cancellationToken);
        }

        public async Task<
            IEnumerable<ReceivingInspectionRequest>
        > GetAcceptedInspectionRequestsAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(r => r.AcceptedDate != null)
                .ToListAsync(cancellationToken);
        }

        public async Task<
            IEnumerable<ReceivingInspectionRequest>
        > GetRejectedInspectionRequestsAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(r => r.RejectionDate != null)
                .ToListAsync(cancellationToken);
        }
    }
}
