using Application.DTOs.Personnel;
using FluentValidation;

namespace Application.Validators.Personnel
{
    public class BonusRequestValidator : AbstractValidator<BonusRequest>
    {
        public BonusRequestValidator()
        {
            // Validar que al menos uno de TechnicianId o TechnicianName esté presente
            RuleFor(x => x)
                .Must(x => (x.TechnicianId.HasValue && x.TechnicianId.Value > 0) || !string.IsNullOrWhiteSpace(x.TechnicianName))
                .WithMessage("Se requiere TechnicianId o TechnicianName");

            // Validar que al menos uno de SuperiorId o SuperiorUsername esté presente
            RuleFor(x => x)
                .Must(x => (x.SuperiorId.HasValue && x.SuperiorId.Value > 0) || !string.IsNullOrWhiteSpace(x.SuperiorUsername))
                .WithMessage("Se requiere SuperiorId o SuperiorUsername");

            RuleFor(x => x.Amount)
                .GreaterThan(0)
                .WithMessage("El valor del bono debe ser mayor a 0");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("La descripción es obligatoria")
                .MaximumLength(500)
                .WithMessage("La descripción no puede exceder 500 caracteres");
        }
    }
}
