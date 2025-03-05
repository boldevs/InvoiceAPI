using FluentValidation;

namespace Application.Features.InvoicesLine.DeleteByInvoiceId
{
    internal sealed class DeleteInvoiceLineCommandValidator : AbstractValidator<DeleteInvoiceLineCommand>
    {
        public DeleteInvoiceLineCommandValidator()
        {
            RuleFor(c => c.invoiceId).NotEmpty();
        }
    }
}
