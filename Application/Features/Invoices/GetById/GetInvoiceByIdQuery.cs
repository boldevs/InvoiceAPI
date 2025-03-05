using Application.Abstractions.Messaging;

namespace Application.Features.Invoices.GetById
{
    public sealed record GetInvoiceByIdQuery(Guid invoiceId) : IQuery<InvoiceResponse>;
}
