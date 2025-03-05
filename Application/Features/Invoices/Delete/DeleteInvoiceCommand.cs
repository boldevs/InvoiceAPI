using Application.Abstractions.Messaging;

namespace Application.Features.Invoices.Delete
{
    public sealed record DeleteInvoiceCommand(Guid invoiceId) : ICommand;
}
