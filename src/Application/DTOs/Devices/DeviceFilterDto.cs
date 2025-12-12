using Domain.Enums;

namespace Application.DTOs.DevicesDTOs
{
    public class DeviceFilterDto
    {
        public DeviceType? DeviceType { get; set; }
        public OperationalState? OperationalState { get; set; }
        public int? DepartmentId { get; set; }

        // Pagination
        public int PageNumber { get; set; } = 0;
        public int PageSize { get; set; } = 20;

        // Order
        public string? OrderBy { get; set; }
        public bool IsDescending { get; set; } = false;
    }
}
