using Application.DTOs.Auth;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Services.Implementations
{
    public class UserManagementService : IUserManagementService
    {
        private readonly IUserRepository _userRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMapper _mapper;
        private readonly ILogger<UserManagementService> _logger;

        public UserManagementService(
            IUserRepository userRepository,
            IDepartmentRepository departmentRepository,
            IUnitOfWork unitOfWork,
            IPasswordHasher passwordHasher,
            IMapper mapper,
            ILogger<UserManagementService> logger
        )
        {
            _userRepository =
                userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _departmentRepository =
                departmentRepository
                ?? throw new ArgumentNullException(nameof(departmentRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _passwordHasher =
                passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<UserDto> CreateUserAsync(
            CreateUserRequestDto request,
            int administratorId,
            CancellationToken cancellationToken = default
        )
        {
            _logger.LogInformation(
                $"Administrator {administratorId} attempting to create user: {request.FullName}"
            );

            // Verify administrator role
            await ValidateAdministratorRoleAsync(administratorId, cancellationToken);

            if (await _userRepository.UsernameExistsAsync(request.Username, cancellationToken))
            {
                _logger.LogWarning(
                    "User creation failed: Username already exists: {Username}",
                    request.Username
                );
                throw new DuplicateEntityException(
                    $"Ya existe un usuario con el nombre de usuario '{request.Username}'"
                );
            }

            // Verify department exists
            var department = await _departmentRepository.GetDepartmentByNameAsync(
                request.DepartmentName,
                cancellationToken
            );
            if (department == null)
            {
                _logger.LogWarning(
                    "User creation failed: Department not found: {DepartmentId}",
                    request.DepartmentName
                );
                throw new EntityNotFoundException("Departamento", request.DepartmentName);
            }

            // Get role Id from role name
            var roleId = GetRoleIdFromName(request.Role);

            // Hash password
            var passwordHash = _passwordHasher.HashPassword(request.Password);

            // Create user entity
            User user;
            if (request.Role == "Technician")
            {
                // Create technician with additional fields
                if (
                    !request.YearsOfExperience.HasValue
                    || string.IsNullOrWhiteSpace(request.Specialty)
                )
                {
                    throw new UserValidationException(
                        "Los años de experiencia y especialidad son obligatorios para técnicos"
                    );
                }

                user = User.CreateTechnician(
                    request.Username,
                    request.FullName,
                    passwordHash,
                    department.DepartmentId,
                    request.YearsOfExperience.Value,
                    request.Specialty
                );
            }
            else
            {
                // Create regular user
                user = new User(
                    request.Username,
                    request.FullName,
                    passwordHash,
                    roleId,
                    department.DepartmentId
                );
            }

            // Save user
            await _userRepository.AddAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "User created successfully: {UserId}, Role: {Role}",
                user.UserId,
                request.Role
            );

            // Reload user with navigation properties
            var reloadedUser = await _userRepository.GetByIdAsync(user.UserId, cancellationToken);

            if (reloadedUser == null)
            {
                throw new EntityNotFoundException("Usuario", user.UserId);
            }

            return _mapper.Map<UserDto>(reloadedUser);
        }

        public async Task<UserDto> UpdateUserAsync(
            UpdateUserRequestDto request,
            int administratorId,
            CancellationToken cancellationToken = default
        )
        {
            _logger.LogInformation(
                "Administrator {AdministratorId} attempting to update user: {UserId}",
                administratorId,
                request.UserId
            );

            // Verify administrator role
            await ValidateAdministratorRoleAsync(administratorId, cancellationToken);

            // Get existing user
            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
            if (user == null)
            {
                _logger.LogWarning("User update failed: User not found: {UserId}", request.UserId);
                throw new EntityNotFoundException("Usuario", request.UserId);
            }

            // Update full name if provided
            if (!string.IsNullOrEmpty(request.FullName))
            {
                user.UpdateProfile(request.FullName, request.Specialty);
            }

            // Update role if provided
            if (!string.IsNullOrEmpty(request.Role))
            {
                var newRoleId = GetRoleIdFromName(request.Role);
                user.ChangeRole(newRoleId);

                // Update technician data if role is Technician
                if (
                    request.Role == "Technician"
                    && request.YearsOfExperience.HasValue
                    && !string.IsNullOrEmpty(request.Specialty)
                )
                {
                    user.SetTechnicalExperience(request.YearsOfExperience.Value, request.Specialty);
                }
            }
            else if (
                request.YearsOfExperience.HasValue
                && !string.IsNullOrEmpty(request.Specialty)
                && user.IsTechnician
            )
            {
                // Update technician data without changing role
                user.SetTechnicalExperience(request.YearsOfExperience.Value, request.Specialty);
            }

            // Update department if provided
            if (request.DepartmentName!=null)
            {
                var department = await _departmentRepository.GetDepartmentByNameAsync(
                    request.DepartmentName,
                    cancellationToken
                );
                if (department == null)
                {
                    _logger.LogWarning(
                        "User update failed: Department not found: {DepartmentId}",
                        request.DepartmentName
                    );
                    throw new EntityNotFoundException("Departamento", request.DepartmentName);
                }
                user.ChangeDepartment(department.DepartmentId);
            }

            // Update active status if provided
            if (request.IsActive.HasValue)
            {
                if (request.IsActive.Value)
                {
                    user.Activate();
                }
                else
                {
                    user.Deactivate();
                }
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("User updated successfully: {UserId}", request.UserId);

            // Reload user with navigation properties
            user = await _userRepository.GetByIdAsync(user.UserId, cancellationToken);

            return _mapper.Map<UserDto>(user!);
        }

        public async Task DeactivateUserAsync(
            DeactivateUserRequestDto request,
            int administratorId,
            CancellationToken cancellationToken = default
        )
        {
            _logger.LogInformation(
                "Administrator {AdministratorId} attempting to deactivate user: {UserId}, Reason: {Reason}",
                administratorId,
                request.UserId,
                request.Reason
            );

            // Verify administrator role
            await ValidateAdministratorRoleAsync(administratorId, cancellationToken);

            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
            if (user == null)
            {
                _logger.LogWarning(
                    "User deactivation failed: User not found: {UserId}",
                    request.UserId
                );
                throw new EntityNotFoundException("Usuario", request.UserId);
            }

            // Prevent deactivating the last administrator
            if (user.IsAdministrator)
            {
                var activeAdmins = (
                    await _userRepository.GetUsersByDepartmentAsync(
                        user.DepartmentId,
                        cancellationToken
                    )
                ).Where(u => u.IsAdministrator && u.IsActive);

                if (activeAdmins.Count() <= 1)
                {
                    _logger.LogWarning(
                        "User deactivation failed: Cannot deactivate last administrator"
                    );
                    throw new BusinessRuleViolationException(
                        "No se puede desactivar al último administrador del sistema"
                    );
                }
            }

            user.Deactivate();
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("User deactivated successfully: {UserId}", request.UserId);
        }

        public async Task ActivateUserAsync(
            int userId,
            int administratorId,
            CancellationToken cancellationToken = default
        )
        {
            _logger.LogInformation(
                "Administrator {AdministratorId} attempting to activate user: {UserId}",
                administratorId,
                userId
            );

            // Verify administrator role
            await ValidateAdministratorRoleAsync(administratorId, cancellationToken);

            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
            if (user == null)
            {
                _logger.LogWarning("User activation failed: User not found: {UserId}", userId);
                throw new EntityNotFoundException("Usuario", userId);
            }

            user.Activate();
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("User activated successfully: {UserId}", userId);
        }

        public async Task<UserDto> GetUserByIdAsync(
            int userId,
            CancellationToken cancellationToken = default
        )
        {
            _logger.LogDebug("Retrieving user: {UserId}", userId);

            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
            if (user == null)
            {
                _logger.LogWarning("User not found: {UserId}", userId);
                throw new EntityNotFoundException("Usuario", userId);
            }

            return _mapper.Map<UserDto>(user);
        }
        public async Task<IEnumerable<UserDto>> GetAllInActiveUsersAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Retrieving all active users");

            var users = await _userRepository.GetAllUsersWithDetailsAsync(cancellationToken);
            var inActiveUsers = users.Where(u => !u.IsActive);

            return _mapper.Map<IEnumerable<UserDto>>(inActiveUsers);
        }
        public async Task<IEnumerable<UserDto>> GetAllActiveUsersAsync(
            CancellationToken cancellationToken = default
        )
        {
            _logger.LogDebug("Retrieving all active users");

            var users = await _userRepository.GetAllUsersWithDetailsAsync(cancellationToken);
            var activeUsers = users.Where(u => u.IsActive);

            return _mapper.Map<IEnumerable<UserDto>>(activeUsers);
        }

        public async Task<IEnumerable<UserDto>> GetUsersByRoleAsync(
            string roleName,
            CancellationToken cancellationToken = default
        )
        {
            _logger.LogDebug("Retrieving users by role: {RoleName}", roleName);

            var roleId = GetRoleIdFromName(roleName);
            var users = await _userRepository.GetAllAsync(cancellationToken);
            var filteredUsers = users.Where(u => u.RoleId == roleId);

            return _mapper.Map<IEnumerable<UserDto>>(filteredUsers);
        }

        public async Task<IEnumerable<UserDto>> GetUsersByDepartmentAsync(
            int departmentId,
            CancellationToken cancellationToken = default
        )
        {
            _logger.LogDebug("Retrieving users by department: {DepartmentId}", departmentId);

            var users = await _userRepository.GetUsersByDepartmentAsync(
                departmentId,
                cancellationToken
            );

            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        private async Task ValidateAdministratorRoleAsync(
            int administratorId,
            CancellationToken cancellationToken
        )
        {
            var administrator = await _userRepository.GetByIdAsync(
                administratorId,
                cancellationToken
            );

            if (administrator == null || !administrator.IsActive)
            {
                _logger.LogWarning(
                    "Access denied: Administrator not found or inactive: {AdministratorId}",
                    administratorId
                );
                throw new AccessDeniedException("Administrador no encontrado o inactivo");
            }

            if (!administrator.IsAdministrator)
            {
                _logger.LogWarning(
                    "Access denied: User is not an administrator: {UserId}, Role: {Role}",
                    administratorId,
                    administrator.Role.Name
                );
                throw new AccessDeniedException(
                    "Solo los administradores pueden realizar esta operación",
                    "Administrator",
                    administrator.Role.Name
                );
            }
        }

        private int GetRoleIdFromName(string roleName)
        {
            return roleName switch
            {
                "Technician" => (int)RoleEnum.Technician,
                "EquipmentReceiver" => (int)RoleEnum.EquipmentReceiver,
                "SectionManager" => (int)RoleEnum.SectionManager,
                "Administrator" => (int)RoleEnum.Administrator,
                "Director" => (int)RoleEnum.Director,
                "Logistician" => (int)RoleEnum.Logistician,
                _ => throw new RoleValidationException($"Rol inválido: {roleName}"),
            };
        }
    }
}
