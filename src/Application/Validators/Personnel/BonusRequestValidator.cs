using Application.DTOs.Personnel;
using FluentValidation;

namespace Application.Validators.Personnel
{
    public class BonusRequestValidator : AbstractValidator<BonusRequest>
    {
        public BonusRequestValidator()
        {
            RuleFor(x => x.Bonus)
                .InclusiveBetween(0, 5)
                .WithMessage("El valor del bono debe estar entre 0 y 5");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("La descripción es obligatoria")
                .MaximumLength(500)
                .WithMessage("La descripción no puede exceder 500 caracteres");

        }
    }
}
