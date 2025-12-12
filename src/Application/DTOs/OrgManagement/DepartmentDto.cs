public class DepartmentDto
{
    public int SectionId { get; set; }
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
    public string SectionName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DepartmentDto(int sectionId, int departmentId, string departmentName, string sectionName, bool isActive)
    {
        SectionId = sectionId;
        DepartmentId = departmentId;
        DepartmentName = departmentName;
        SectionName = sectionName;
        IsActive = isActive;
    }
}

public class CreateDepartmentDto
{
    public int SectionId { get; set; }
    public string Name { get; set; } = string.Empty;
}