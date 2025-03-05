using Application.Abstractions.Messaging;

namespace Application.Features.InvoicesLine.GetById
{
    public sealed record GetInvoiceLineByIdQuery(Guid invoiceLineId) : IQuery<InvoiceLineResponse>;
}
