using FluentValidation;

namespace Application.Features.Customers.Update
{
    internal sealed class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
    {
        public UpdateCustomerCommandValidator()
        {
            RuleFor(c => c.CustomerId)
                .NotEmpty()
                .WithMessage("CustomerId is required.");

            RuleFor(c => c.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("Invalid email format.");

            RuleFor(c => c.Name)
                .NotEmpty()
                .WithMessage("Name is required.")
                .MinimumLength(2)
                .WithMessage("Name must be at least 2 characters long.");

            RuleFor(c => c.Phone)
                .NotEmpty()
                .WithMessage("Phone number is required.")
                .Matches(@"^\+?[1-9]\d{1,14}$")  // E.164 format
                .WithMessage("Invalid phone number format.");

            RuleFor(c => c.Address)
                .NotEmpty()
                .WithMessage("Address is required.")
                .MinimumLength(5)
                .WithMessage("Address must be at least 5 characters long.");
        }
    }
}
