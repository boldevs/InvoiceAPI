using FluentValidation;

namespace Application.Features.Users.Register
{
    internal sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(u => u.Email)
                .NotEmpty()
                .MaximumLength(255)
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(u => u.Password)
                .NotEmpty()
                .MinimumLength(6)
                .WithMessage("Password must be at least 6 characters.");

            RuleFor(u => u.FirstName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(u => u.LastName)
                .NotEmpty()
                .MaximumLength(100);
        }
    }
}
