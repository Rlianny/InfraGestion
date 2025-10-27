using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IDeviceReceiverRepository : IRepository<DeviceReceiver>
    {
        Task<IEnumerable<DeviceReceiver>> GetDeviceReceiversByDepartmentAsync(int departmentId, CancellationToken cancellationToken = default);
    }
}
