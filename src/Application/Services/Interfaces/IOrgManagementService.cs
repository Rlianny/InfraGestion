namespace Application.Services.Interfaces
{
    public interface IOrgManagementService
    {
        Task<IEnumerable<SectionDto>> GetSectionsAsync();
        Task<IEnumerable<DepartmentDto>> GetDepartmentsAsync();
        Task CreateSection(SectionDto sectionDto); 
        Task ModifySection(SectionDto sectionDto); 
        Task DisableSection(SectionDto sectionDto);
        Task CreateDepartment(DepartmentDto departmentDto); 
        Task ModifyDepartment(DepartmentDto departmentDto);
        Task DisableDepartment(DepartmentDto departmentDto); 
        Task AssignSectionResponsible(AssignSectionResponsibleDto assignSectionResponsible);
    }
}