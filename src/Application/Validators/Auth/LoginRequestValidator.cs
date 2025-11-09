using Application.DTOs.Auth;
using FluentValidation;

namespace Application.Validators.Auth
{
    public class LoginRequestValidator : AbstractValidator<LoginRequestDto>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty()
                .WithMessage("El identificador es obligatorio")
                .MaximumLength(100)
                .WithMessage("El identificador no puede exceder 100 caracteres");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("La contraseña es obligatoria")
                .MinimumLength(6)
                .WithMessage("La contraseña debe tener al menos 6 caracteres");
        }
    }
}
