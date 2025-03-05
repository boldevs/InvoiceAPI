using FluentValidation;

namespace Application.Features.InvoicesLine.Create
{
    internal sealed class CreateInvoiceLineCommandValidator : AbstractValidator<CreateInvoiceLineCommand>
    {
        public CreateInvoiceLineCommandValidator()
        {
            RuleFor(c => c.InvoiceId)
                .NotEmpty().WithMessage("InvoiceId is required.")
                .NotEqual(Guid.Empty).WithMessage("InvoiceId must be a valid GUID.");

            RuleFor(c => c.ItemId)
                .NotEmpty().WithMessage("ItemId is required.")
                .NotEqual(Guid.Empty).WithMessage("User ID must be a valid GUID.");

            RuleFor(c => c.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.");

            RuleFor(c => c.UnitPrice)
                .GreaterThan(0).WithMessage("UnitPrice must be greater than zero.");
        }
    }
}
