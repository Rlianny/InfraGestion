using Application.DTOs.Auth;

namespace Application.Services.Interfaces
{
    public interface IOrgManagementService
    {
        Task<IEnumerable<SectionDto>> GetSectionsAsync();
        Task<IEnumerable<DepartmentDto>> GetDepartmentsAsync();
        Task CreateSection(SectionDto sectionDto); 
        Task ModifySection(SectionDto sectionDto); 
        Task DisableSection(int sectionId);
        Task DeleteSection(int sectionId);
        Task CreateDepartment(DepartmentDto departmentDto); 
        Task ModifyDepartment(DepartmentDto departmentDto);
        Task DisableDepartment(int departmentId);
        Task DeleteDepartment(int departmentId);
        Task AssignSectionResponsible(AssignSectionResponsibleDto assignSectionResponsible);
    }
}