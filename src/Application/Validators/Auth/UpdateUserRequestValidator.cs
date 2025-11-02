using Application.DTOs.Auth;
using FluentValidation;

namespace Application.Validators.Auth
{
    
    
    
    public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequestDto>
    {
        private static readonly string[] ValidRoles = 
        {
            "Technician",
            "EquipmentReceiver",
            "SectionManager",
            "Administrator",
            "Director"
        };

        public UpdateUserRequestValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("El ID de usuario es obligatorio");

            When(x => !string.IsNullOrEmpty(x.FullName), () =>
            {
                RuleFor(x => x.FullName)
                    .MinimumLength(3).WithMessage("El nombre completo debe tener al menos 3 caracteres")
                    .MaximumLength(100).WithMessage("El nombre completo no puede exceder 100 caracteres");
            });

            When(x => !string.IsNullOrEmpty(x.Role), () =>
            {
                RuleFor(x => x.Role)
                    .Must(role => ValidRoles.Contains(role!)).WithMessage("El rol especificado no es válido");
            });

            When(x => x.DepartmentId.HasValue, () =>
            {
                RuleFor(x => x.DepartmentId)
                    .GreaterThan(0).WithMessage("El ID de departamento debe ser válido");
            });

            When(x => x.YearsOfExperience.HasValue, () =>
            {
                RuleFor(x => x.YearsOfExperience)
                    .GreaterThanOrEqualTo(0).WithMessage("Los años de experiencia no pueden ser negativos")
                    .LessThanOrEqualTo(50).WithMessage("Los años de experiencia no pueden exceder 50");
            });

            When(x => !string.IsNullOrEmpty(x.Specialty), () =>
            {
                RuleFor(x => x.Specialty)
                    .MaximumLength(100).WithMessage("La especialidad no puede exceder 100 caracteres");
            });
        }
    }
}
