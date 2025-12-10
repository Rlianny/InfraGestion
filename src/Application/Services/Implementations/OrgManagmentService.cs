using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs.Auth;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Application.Services.Implementations
{
    public class OrgManagmentService : IOrgManagementService
    {
        private readonly ISectionRepository sectionRepository;
        private readonly IDepartmentRepository departmentRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IUserRepository userRepository;

        public OrgManagmentService(
            ISectionRepository sectionRepository,
            IDepartmentRepository departmentRepository,
            IUnitOfWork unitOfWork,
            IUserRepository userRepository
        )
        {
            this.sectionRepository = sectionRepository;
            this.departmentRepository = departmentRepository;
            this.unitOfWork = unitOfWork;
            this.userRepository = userRepository;
        }

        public async Task CreateDepartment(CreateDepartmentDto createDepartmentDto)
        {
            var section = await sectionRepository.GetByIdAsync(createDepartmentDto.SectionId);
            if (section == null)
            {
                throw new EntityNotFoundException("Section", createDepartmentDto.SectionId);
            }

            var exists = await departmentRepository.GetDepartmentByNameAsync(createDepartmentDto.Name);
            if (exists != null)
            {
                throw new DuplicateEntityException("Department", "Name", createDepartmentDto.Name);
            }

            await departmentRepository.AddAsync(
                new Department(createDepartmentDto.Name, createDepartmentDto.SectionId)
            );
            await unitOfWork.SaveChangesAsync();
        }

        public async Task CreateSection(CreateSectionDto createSectionDto)
        {
            var exists = await sectionRepository.GetSectionByNameAsync(createSectionDto.Name);
            if (exists != null)
            {
                throw new DuplicateEntityException("Section", "Name", createSectionDto.Name);
            }

            if (string.IsNullOrWhiteSpace(createSectionDto.Name))
            {
                throw new ArgumentException("El nombre de la sección no puede estar vacío", nameof(createSectionDto.Name));
            }

            var section = new Section(createSectionDto.Name);

            if (createSectionDto.SectionManagerId.HasValue)
            {
                var manager =
                    await userRepository.GetByIdAsync(createSectionDto.SectionManagerId.Value)
                    ?? throw new EntityNotFoundException("User", createSectionDto.SectionManagerId.Value);

                if (manager.RoleId != (int)RoleEnum.SectionManager)
                {
                    throw new BusinessRuleViolationException(
                        "El usuario asignado como manager debe tener el rol SectionManager."
                    );
                }

                section.AssignManager(manager);
            }

            await sectionRepository.AddAsync(section);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task DisableDepartment(int departmentId)
        {
            var department =
                await departmentRepository.GetByIdAsync(departmentId)
                ?? throw new EntityNotFoundException("Department", departmentId);
            department.Disable();
            await departmentRepository.UpdateAsync(department);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task DisableSection(int sectionId)
        {
            var section =
                await sectionRepository.GetByIdAsync(sectionId)
                ?? throw new EntityNotFoundException("Section", sectionId);
            section.Disable();
            await sectionRepository.UpdateAsync(section);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task ModifyDepartment(DepartmentDto departmentDto)
        {
            var department =
                await departmentRepository.GetByIdAsync(departmentDto.DepartmentId)
                ?? throw new EntityNotFoundException("Department", departmentDto.DepartmentId);

            if (department.SectionId != departmentDto.SectionId)
            {
                var section = await sectionRepository.GetByIdAsync(departmentDto.SectionId);
                if (section == null)
                {
                    throw new EntityNotFoundException("Section", departmentDto.SectionId);
                }
            }

            if (department.Name != departmentDto.Name)
            {
                var exists = await departmentRepository.GetDepartmentByNameAsync(
                    departmentDto.Name
                );
                if (exists != null)
                {
                    throw new DuplicateEntityException("Department", "Name", departmentDto.Name);
                }
            }

            department.UpdateName(departmentDto.Name);
            department.UpdateSection(departmentDto.SectionId);

            await departmentRepository.UpdateAsync(department);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task ModifySection(SectionDto sectionDto)
        {
            var section =
                await sectionRepository.GetByIdAsync(sectionDto.SectionId)
                ?? throw new EntityNotFoundException("Section", sectionDto.SectionId);

            if (section.Name != sectionDto.Name)
            {
                var exists = await sectionRepository.SectionExistsAsync(sectionDto.Name);
                if (exists)
                {
                    throw new DuplicateEntityException("Section", "Name", sectionDto.Name);
                }
            }

            section.UpdateName(sectionDto.Name);

            if (sectionDto.SectionManagerId.HasValue)
            {
                var manager =
                    await userRepository.GetByIdAsync(sectionDto.SectionManagerId.Value)
                    ?? throw new EntityNotFoundException("User", sectionDto.SectionManagerId.Value);

                if (manager.RoleId != (int)RoleEnum.SectionManager)
                    throw new BusinessRuleViolationException(
                        "El usuario asignado como manager debe tener el rol SectionManager."
                    );

                section.AssignManager(manager);
            }

            await sectionRepository.UpdateAsync(section);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<SectionDto>> GetSectionsAsync()
        {
            var sections = await sectionRepository.GetAllAsync();
            var sectionDtos = new List<SectionDto>();
            foreach (var section in sections)
            {
                User manager = null;
                if (section.SectionManagerId != null)
                {
                    manager = await userRepository.GetByIdAsync((int)section.SectionManagerId);

                }
                var dto = new SectionDto
                {
                    SectionId = section.SectionId,
                    Name = section.Name,
                    SectionManagerId = manager == null ? null : manager.UserId,
                    SectionManagerFullName = manager == null ? "" : manager.FullName
                };
                sectionDtos.Add(dto);
            }
            return sectionDtos;
        }

        public async Task<IEnumerable<DepartmentDto>> GetDepartmentsAsync()
        {
            var departments = await departmentRepository.GetAllAsync();
            var departmentDtos = new List<DepartmentDto>();
            foreach (var department in departments)
            {
                var dto = new DepartmentDto
                {
                    DepartmentId = department.DepartmentId,
                    Name = department.Name,
                    SectionId = department.SectionId,
                };
                departmentDtos.Add(dto);
            }
            return departmentDtos;
        }

        public async Task DeleteSection(int sectionId)
        {
            var section =
                await sectionRepository.GetByIdAsync(sectionId)
                ?? throw new EntityNotFoundException("Section", sectionId);
            await sectionRepository.DeleteAsync(section);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteDepartment(int departmentId)
        {
            var department =
                await departmentRepository.GetByIdAsync(departmentId)
                ?? throw new EntityNotFoundException("Department", departmentId);
            await departmentRepository.DeleteAsync(department);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserDto>> GetSectionManagersAsync()
        {
            var managers = await userRepository.GetUsersByRoleAsync((int)RoleEnum.SectionManager);

            var managerDtos = new List<UserDto>();
            foreach (var manager in managers)
            {
                managerDtos.Add(
                    new UserDto
                    {
                        UserId = manager.UserId,
                        Username = manager.Username,
                        FullName = manager.FullName,
                        Role = manager.Role?.Name ?? "SectionManager",
                        DepartmentId = manager.DepartmentId,
                        DepartmentName = manager.Department?.Name ?? string.Empty,
                        IsActive = manager.IsActive,
                        CreatedAt = manager.CreatedAt,
                        YearsOfExperience = manager.YearsOfExperience,
                        Specialty = manager.Specialty,
                    }
                );
            }

            return managerDtos;
        }
    }
}
