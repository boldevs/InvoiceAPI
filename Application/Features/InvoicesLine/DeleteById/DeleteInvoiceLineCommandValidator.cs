using FluentValidation;

namespace Application.Features.InvoicesLine.DeleteById
{
    internal sealed class DeleteInvoiceLineCommandValidator : AbstractValidator<DeleteInvoiceLineCommand>
    {
        public DeleteInvoiceLineCommandValidator()
        {
            RuleFor(c => c.Id).NotEmpty();
        }
    }
}
