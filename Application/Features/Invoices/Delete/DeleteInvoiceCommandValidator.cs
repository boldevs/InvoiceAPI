using FluentValidation;

namespace Application.Features.Invoices.Delete
{
    internal sealed class DeleteInvoiceCommandValidator : AbstractValidator<DeleteInvoiceCommand>
    {
        public DeleteInvoiceCommandValidator()
        {
            RuleFor(c => c.invoiceId).NotEmpty();
        }
    }
}
