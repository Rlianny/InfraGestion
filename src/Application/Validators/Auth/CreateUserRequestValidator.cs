using Application.DTOs.Auth;
using FluentValidation;

namespace Application.Validators.Auth
{
    public class CreateUserRequestValidator : AbstractValidator<CreateUserRequestDto>
    {
        private static readonly string[] ValidRoles =
        {
            "Technician",
            "EquipmentReceiver",
            "SectionManager",
            "Administrator",
            "Director",
        };

        public CreateUserRequestValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty()
                .WithMessage("El nombre completo es obligatorio")
                .MinimumLength(3)
                .WithMessage("El nombre completo debe tener al menos 3 caracteres")
                .MaximumLength(100)
                .WithMessage("El nombre completo no puede exceder 100 caracteres");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("La contraseña es obligatoria")
                .MinimumLength(8)
                .WithMessage("La contraseña debe tener al menos 8 caracteres")
                .Matches(@"[A-Z]")
                .WithMessage("La contraseña debe contener al menos una mayúscula")
                .Matches(@"[a-z]")
                .WithMessage("La contraseña debe contener al menos una minúscula")
                .Matches(@"[0-9]")
                .WithMessage("La contraseña debe contener al menos un número");

            RuleFor(x => x.Role)
                .NotEmpty()
                .WithMessage("El rol es obligatorio")
                .Must(role => ValidRoles.Contains(role))
                .WithMessage("El rol especificado no es válido");

            RuleFor(x => x.DepartmentId)
                .GreaterThan(0)
                .WithMessage("El departamento es obligatorio");

            // Technician-specific validation
            When(
                x => x.Role == "Technician",
                () =>
                {
                    RuleFor(x => x.YearsOfExperience)
                        .NotNull()
                        .WithMessage("Los años de experiencia son obligatorios para técnicos")
                        .GreaterThanOrEqualTo(0)
                        .WithMessage("Los años de experiencia no pueden ser negativos");

                    RuleFor(x => x.Specialty)
                        .NotEmpty()
                        .WithMessage("La especialidad es obligatoria para técnicos")
                        .MaximumLength(100)
                        .WithMessage("La especialidad no puede exceder 100 caracteres");
                }
            );

            // Section Manager validation
            When(
                x => x.Role == "SectionManager",
                () =>
                {
                    RuleFor(x => x.SectionId)
                        .NotNull()
                        .WithMessage("La sección es obligatoria para responsables de sección")
                        .GreaterThan(0)
                        .WithMessage("La sección debe ser válida");
                }
            );
        }
    }
}
