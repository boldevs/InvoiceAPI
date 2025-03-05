using Application.Abstractions.Messaging;

namespace Application.Features.InvoicesLine.DeleteByInvoiceId
{
    public sealed record DeleteInvoiceLineCommand(Guid invoiceId) : ICommand;
}
