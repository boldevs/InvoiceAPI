using Application.Abstractions.Messaging;

namespace Application.Features.InvoicesLine.DeleteById
{
    public sealed record DeleteInvoiceLineCommand(Guid Id) : ICommand;
}
