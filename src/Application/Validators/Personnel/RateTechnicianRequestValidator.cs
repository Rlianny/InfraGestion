using Application.DTOs.Personnel;
using FluentValidation;

namespace Application.Validators.Personnel
{
    public class RateTechnicianRequestValidator : AbstractValidator<RateTechnicianRequest>
    {
        public RateTechnicianRequestValidator()
        {
            // Validar que al menos uno de TechnicianId o TechnicianName esté presente
            RuleFor(x => x)
                .Must(x => (x.TechnicianId.HasValue && x.TechnicianId.Value > 0) || !string.IsNullOrWhiteSpace(x.TechnicianName))
                .WithMessage("Se requiere TechnicianId o TechnicianName");

            // Validar que al menos uno de SuperiorId o SuperiorUsername esté presente
            RuleFor(x => x)
                .Must(x => (x.SuperiorId.HasValue && x.SuperiorId.Value > 0) || !string.IsNullOrWhiteSpace(x.SuperiorUsername))
                .WithMessage("Se requiere SuperiorId o SuperiorUsername");

            RuleFor(x => x.Rate)
                .InclusiveBetween(0, 5)
                .WithMessage("La calificación debe estar entre 0 y 5");

            RuleFor(x => x.Comments)
                .MaximumLength(500)
                .WithMessage("Los comentarios no pueden exceder 500 caracteres");
        }
    }
}
