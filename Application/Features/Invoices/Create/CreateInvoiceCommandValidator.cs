using FluentValidation;

namespace Application.Features.Invoices.Create
{
    internal sealed class CreateInvoiceCommandValidator : AbstractValidator<CreateInvoiceCommand>
    {
        public CreateInvoiceCommandValidator()
        {
            RuleFor(c => c.InvoiceNumber)
                .NotEmpty().WithMessage("Invoice number is required.")
                .MaximumLength(50).WithMessage("Invoice number cannot exceed 50 characters.");

            RuleFor(c => c.UserId)
                .NotEmpty().WithMessage("User ID is required.")
                .NotEqual(Guid.Empty).WithMessage("User ID must be a valid GUID.");

            RuleFor(c => c.CustomerId)
                .NotEmpty().WithMessage("Customer ID is required.")
                .NotEqual(Guid.Empty).WithMessage("Customer ID must be a valid GUID.");

            RuleFor(c => c.IssuedDate)
                .NotEmpty().WithMessage("Issued date is required.")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Issued date cannot be in the future.");

            RuleFor(c => c.DueDate)
                .NotEmpty().WithMessage("Due date is required.")
                .GreaterThan(c => c.IssuedDate).WithMessage("Due date must be after the issued date.");

            RuleFor(c => c.TotalAmount)
                .GreaterThan(0).WithMessage("Total amount must be greater than zero.");
        }
    }
}
