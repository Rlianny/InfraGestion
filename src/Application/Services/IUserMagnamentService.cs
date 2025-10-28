public interface IUserManagementService
{
    Task<UserDto> GetUserByIdAsync(string userId);
    Task<List<UserDto>> GetUsersByRoleAsync(UserRole role);
    Task<List<UserDto>> GetUsersByDepartmentAsync(string departmentId);
    Task AssignUserToSectionAsync(string userId, string sectionId);
}