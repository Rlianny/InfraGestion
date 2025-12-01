using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Application.Services.Interfaces;
using Domain.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Domain.Exceptions;
using Domain.Enums;
namespace Application.Services.Implementations
{
    public class OrgManagmentService : IOrgManagementService
    {
        private readonly ISectionRepository sectionRepository;
        private readonly IDepartmentRepository departmentRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IUserRepository userRepository;

        public OrgManagmentService(ISectionRepository sectionRepository, IDepartmentRepository departmentRepository, IUnitOfWork unitOfWork,
            IUserRepository userRepository)
        {
            this.sectionRepository = sectionRepository;
            this.departmentRepository = departmentRepository;
            this.unitOfWork = unitOfWork;
            this.userRepository = userRepository;
        }

        public async Task AssignSectionResponsible(AssignSectionResponsibleDto assignSectionResponsible)
        {
            var section = await sectionRepository.GetByIdAsync(assignSectionResponsible.SectionId) ??
                throw new EntityNotFoundException("Section", assignSectionResponsible.SectionId);
            var user = await userRepository.GetByIdAsync(assignSectionResponsible.UserId) ??
                throw new EntityNotFoundException("User", assignSectionResponsible.UserId);
            section.AssignManager(user);
            user.ChangeRole((int)RoleEnum.SectionManager);
            await sectionRepository.UpdateAsync(section);
            await userRepository.UpdateAsync(user);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task CreateDepartment(DepartmentDto departmentDto)
        {
            await departmentRepository.AddAsync(new Department(departmentDto.Name, departmentDto.SectionId));
            await unitOfWork.SaveChangesAsync();
        }

        public async Task CreateSection(SectionDto sectionDto)
        {
            await sectionRepository.AddAsync(new Section(sectionDto.Name));
            await unitOfWork.SaveChangesAsync();
        }

        public async Task DisableDepartment(DepartmentDto departmentDto)
        {
            var department = await departmentRepository.GetByIdAsync(departmentDto.DepartmentId) ??
                throw new EntityNotFoundException("Deparment", departmentDto.DepartmentId);
            await departmentRepository.DeleteAsync(department);
            await unitOfWork.SaveChangesAsync();

        }

        public async Task DisableSection(SectionDto sectionDto)
        {

            var section = await sectionRepository.GetByIdAsync(sectionDto.SectionId) ??
                throw new EntityNotFoundException("Section", sectionDto.SectionId);
            await sectionRepository.DeleteAsync(section);
            await unitOfWork.SaveChangesAsync();

        }

        public async Task ModifyDepartment(DepartmentDto departmentDto)
        {
            var department = await departmentRepository.GetByIdAsync(departmentDto.DepartmentId)
                    ?? throw new EntityNotFoundException("Deparment", departmentDto.DepartmentId);
            await departmentRepository.UpdateAsync(department);
            await unitOfWork.SaveChangesAsync();

        }

        public async Task ModifySection(SectionDto sectionDto)
        {
            var section = await sectionRepository.GetByIdAsync(sectionDto.SectionId) ??
                 throw new EntityNotFoundException("Section", sectionDto.SectionId);
            await sectionRepository.UpdateAsync(section);
            await unitOfWork.SaveChangesAsync();
        }
        public async Task<IEnumerable<SectionDto>> GetSectionsAsync()
        {
            var sections = await sectionRepository.GetAllAsync();
            var sectionDtos = new List<SectionDto>();
            foreach (var section in sections)
            {
                var dto = new SectionDto
                {
                    SectionId = section.SectionId,
                    Name = section.Name
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
                    SectionId = department.SectionId
                };
                departmentDtos.Add(dto);
            }
            return departmentDtos;
        }
    }
}
