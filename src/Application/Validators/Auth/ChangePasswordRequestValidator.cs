using Application.DTOs.Auth;
using FluentValidation;

namespace Application.Validators.Auth
{
    public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequestDto>
    {
        public ChangePasswordRequestValidator()
        {
            RuleFor(x => x.UserId).GreaterThan(0).WithMessage("El Id de usuario es obligatorio");

            RuleFor(x => x.CurrentPassword)
                .NotEmpty()
                .WithMessage("La contraseña actual es obligatoria");

            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .WithMessage("La nueva contraseña es obligatoria")
                .MinimumLength(8)
                .WithMessage("La nueva contraseña debe tener al menos 8 caracteres")
                .Matches(@"[A-Z]")
                .WithMessage("La nueva contraseña debe contener al menos una mayúscula")
                .Matches(@"[a-z]")
                .WithMessage("La nueva contraseña debe contener al menos una minúscula")
                .Matches(@"[0-9]")
                .WithMessage("La nueva contraseña debe contener al menos un número")
                .NotEqual(x => x.CurrentPassword)
                .WithMessage("La nueva contraseña debe ser diferente a la actual");

            RuleFor(x => x.ConfirmNewPassword)
                .Equal(x => x.NewPassword)
                .WithMessage("La confirmación de contraseña no coincide");
        }
    }
}
