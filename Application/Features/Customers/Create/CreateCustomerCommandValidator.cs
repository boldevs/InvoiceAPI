using FluentValidation;

namespace Application.Features.Customers.Create
{
    public sealed class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerCommandValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .MaximumLength(100); // Match DB configuration

            RuleFor(x => x.Phone)
                .Matches(@"^\+[1-9]\d{1,14}$")
                .WithMessage("Phone number must be in valid E.164 format.");

            RuleFor(c => c.Email)
                .NotEmpty()
                .MaximumLength(255)
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(c => c.Address)
                .MaximumLength(300)
                .When(c => !string.IsNullOrEmpty(c.Address)) // Address is optional
                .WithMessage("Address cannot exceed 300 characters.");
        }
    }
}
