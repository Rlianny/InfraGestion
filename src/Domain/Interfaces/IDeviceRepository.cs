using Domain.Entities;
using Domain.Enums;

namespace Domain.Interfaces
{
    public interface IDeviceRepository : IRepository<Device>
    {
        Task<IEnumerable<Device>> GetDevicesByDepartmentAsync(int departmentId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Device>> GetDevicesByTypeAsync(DeviceType type, CancellationToken cancellationToken = default);
        Task<IEnumerable<Device>> GetDevicesByOperationalStateAsync(OperationalState state, CancellationToken cancellationToken = default);
        Task<IEnumerable<Device>> GetDevicesAcquiredBetweenAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
        Task<int> CountDevicesByStateAsync(OperationalState state, CancellationToken cancellationToken = default);
        Task<IEnumerable<Device>> SearchDevicesByNameAsync(string name, CancellationToken cancellationToken = default);
    }
}
