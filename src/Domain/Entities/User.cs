using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Domain.Aggregations;
using Domain.Enums;
using Domain.Exceptions;

namespace Domain.Entities
{
    public class User
    {
        public int UserId { get; private set; }
        public string Username { get; private set; } = string.Empty;
        public string FullName { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;

        public int RoleId { get; private set; }
        public virtual Role Role { get; private set; } = null!;

        public int DepartmentId { get; private set; }
        public virtual Department Department { get; private set; } = null!;

        // Technician only fields
        public int? YearsOfExperience { get; private set; }
        public string? Specialty { get; private set; }

        // Audit and security fields
        public DateTime CreatedAt { get; private set; }
        public bool IsActive { get; private set; }
        public string? RefreshToken { get; private set; }
        public DateTime? RefreshTokenExpiryTime { get; private set; }

        public virtual ICollection<MaintenanceRecord> MaintenancesPerformed { get; private set; } =
            new List<MaintenanceRecord>();
        public virtual ICollection<DecommissioningRequest> DecommissioningsProposed
        {
            get;
            private set;
        } = new List<DecommissioningRequest>();
        public virtual ICollection<PerformanceRating> PerformanceRecords { get; private set; } =
            new List<PerformanceRating>();

        private User() { }

        public User(string username, string fullName, string passwordHash, int roleId, int departmentId)
        {
            ValidateAndSetUsername(username);
            ValidateAndSetFullName(fullName);
            ValidateAndSetPasswordHash(passwordHash);
            ValidateAndSetRole(roleId);

            DepartmentId = departmentId;
            CreatedAt = DateTime.UtcNow;
            IsActive = true;
        }

        public static User CreateTechnician(
            string username,
            string fullName,
            string passwordHash,
            int departmentId,
            int yearsOfExperience,
            string specialty
        )
        {
            var user = new User(username, fullName, passwordHash, (int)RoleEnum.Technician, departmentId);

            user.SetTechnicalExperience(yearsOfExperience, specialty);
            return user;
        }

        private void ValidateAndSetUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new UserValidationException("El nombre de usuario no puede estar vacío");

            if (username.Length < 3)
                throw new UserValidationException(
                    "El nombre de usuario debe tener al menos 3 caracteres"
                );

            if (username.Length > 50)
                throw new UserValidationException(
                    "El nombre de usuario no puede exceder 50 caracteres"
                );

            // Validar que solo contenga caracteres alfanuméricos, guiones y guiones bajos
            if (!Regex.IsMatch(username, @"^[a-zA-Z0-9_-]+$"))
                throw new UserValidationException(
                    "El nombre de usuario solo puede contener letras, números, guiones y guiones bajos"
                );

            Username = username.Trim().ToLower();
        }

        private void ValidateAndSetFullName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                throw new UserValidationException("El nombre completo no puede estar vacío");

            if (fullName.Length < 3)
                throw new UserValidationException(
                    "El nombre completo debe tener al menos 3 caracteres"
                );

            if (fullName.Length > 100)
                throw new UserValidationException(
                    "El nombre completo no puede exceder 100 caracteres"
                );

            FullName = fullName.Trim();
        }

        private void ValidateAndSetPasswordHash(string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new UserValidationException("El hash de contraseña no puede estar vacío");
            if (passwordHash.Length < 20)
                throw new UserValidationException("Hash de contraseña inválido");

            PasswordHash = passwordHash;
        }

        private void ValidateAndSetRole(int roleId)
        {
            if (!Enum.IsDefined(typeof(RoleEnum), roleId))
                throw new UserValidationException($"Rol inválido: {roleId}");

            RoleId = roleId;
        }

        public void SetTechnicalExperience(int years, string specialty)
        {
            if (years < 0 || years > 50)
                throw new UserValidationException(
                    "Los años de experiencia deben estar entre 0 y 50"
                );

            if (string.IsNullOrWhiteSpace(specialty))
                throw new UserValidationException("La especialidad no puede estar vacía");

            if (specialty.Length > 100)
                throw new UserValidationException(
                    "La especialidad no puede exceder 100 caracteres"
                );

            YearsOfExperience = years;
            Specialty = specialty.Trim();
        }

        public void UpdateProfile(string fullName, string? specialty = null)
        {
            ValidateAndSetFullName(fullName);

            if (specialty != null && IsTechnician)
            {
                if (specialty.Length > 100)
                    throw new UserValidationException(
                        "La especialidad no puede exceder 100 caracteres"
                    );
                Specialty = specialty.Trim();
            }
        }

        public void ChangePassword(string newPasswordHash)
        {
            ValidateAndSetPasswordHash(newPasswordHash);
        }

        public void ChangeRole(int newRoleId)
        {
            ValidateAndSetRole(newRoleId);
            if (!IsTechnician)
            {
                YearsOfExperience = null;
                Specialty = null;
            }
        }

        public void ChangeDepartment(int newDepartmentId)
        {
            if (newDepartmentId <= 0)
                throw new UserValidationException("Id de departamento inválido");

            DepartmentId = newDepartmentId;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void SetRefreshToken(string token, DateTime expiryTime)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new UserValidationException("Token de actualización inválido");

            if (expiryTime <= DateTime.UtcNow)
                throw new UserValidationException("La fecha de expiración debe ser futura");

            RefreshToken = token;
            RefreshTokenExpiryTime = expiryTime;
        }

        public void ClearRefreshToken()
        {
            RefreshToken = null;
            RefreshTokenExpiryTime = null;
        }

        public bool CanRegisterMaintenance()
        {
            return IsActive && IsTechnician;
        }

        public bool CanProposeDecommissioning()
        {
            return IsActive && IsTechnician;
        }

        public bool CanRequestTransfer()
        {
            return IsActive && IsSectionManager;
        }

        public bool CanManageInventory()
        {
            return IsActive && (IsAdministrator || IsDirector);
        }

        public bool CanGenerateReports()
        {
            return IsActive && IsDirector;
        }

        public bool HasValidRefreshToken()
        {
            return !string.IsNullOrWhiteSpace(RefreshToken)
                && RefreshTokenExpiryTime.HasValue
                && RefreshTokenExpiryTime.Value > DateTime.UtcNow;
        }

        public bool IsTechnician => RoleId == (int)RoleEnum.Technician;
        public bool IsAdministrator => RoleId == (int)RoleEnum.Administrator;
        public bool IsDirector => RoleId == (int)RoleEnum.Director;
        public bool IsSectionManager => RoleId == (int)RoleEnum.SectionManager;
        public bool IsEquipmentReceiver => RoleId == (int)RoleEnum.EquipmentReceiver;
    }
}
