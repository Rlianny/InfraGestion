using Application.DTOs.Auth;
using FluentValidation;

namespace Application.Validators.Auth
{
    public class DeactivateUserRequestValidator : AbstractValidator<DeactivateUserRequestDto>
    {
        public DeactivateUserRequestValidator()
        {
            RuleFor(x => x.UserId).GreaterThan(0).WithMessage("El Id de usuario es obligatorio");
        }
    }
}
