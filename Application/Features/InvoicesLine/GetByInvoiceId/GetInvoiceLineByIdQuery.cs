using Application.Abstractions.Messaging;

namespace Application.Features.InvoicesLine.GetByInvoiceId
{
    public sealed record GetInvoiceLineByIdQuery(Guid invoiceId) : IQuery<List<InvoiceLineResponse>>;
}
