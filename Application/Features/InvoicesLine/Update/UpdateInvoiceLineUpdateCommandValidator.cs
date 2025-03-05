using FluentValidation;

namespace Application.Features.InvoicesLine.Update
{
    internal sealed class UpdateInvoiceLineUpdateCommandValidator : AbstractValidator<UpdateInvoiceLineUpdateCommand>
    {
        public UpdateInvoiceLineUpdateCommandValidator()
        {
            RuleFor(c => c.Id)
                .NotEmpty().WithMessage("Id is required.")
                .NotEqual(Guid.Empty).WithMessage("Id must be a valid GUID.");

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
