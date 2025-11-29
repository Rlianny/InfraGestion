using Application.DTOs.Personnel;
using FluentValidation;

namespace Application.Validators.Personnel
{
    public class RateTechnicianRequestValidator : AbstractValidator<RateTechnicianRequest>
    {
        public RateTechnicianRequestValidator()
        {
            
            RuleFor(x => x.Rate)
                .InclusiveBetween(0, 5)
                .WithMessage("La calificación debe estar entre 0 y 5");

            RuleFor(x => x.Comments)
                .MaximumLength(500)
                .WithMessage("Los comentarios no pueden exceder 500 caracteres");
        }
    }
}
