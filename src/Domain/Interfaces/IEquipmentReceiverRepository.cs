using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IEquipmentReceiverRepository : IRepository<EquipmentReceiver>
    {
        Task<IEnumerable<EquipmentReceiver>> GetEquipmentReceiversByDepartmentAsync(int departmentId, CancellationToken cancellationToken = default);
    }
}
