namespace Application.DTOs.Inventory
{
    public class DeviceFilterDto
    {
        public DeviceType? DeviceType { get; set; }
        public string? OperationalState { get; set; }
        public int? DepartmentId { get; set; }
        public int? SectionId { get; set; }
        public string? SearchTerm { get; set; } // for looking for name or id
        
        // Pagination
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        
        // Order
        public string? OrderBy { get; set; } // "Name", "AcquisitionDate", etc.
        public bool IsDescending { get; set; } = false;
    }
}