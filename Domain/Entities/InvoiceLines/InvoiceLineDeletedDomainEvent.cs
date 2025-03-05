using SharedKernel;

namespace Domain.Entities.InvoiceLines
{
    public sealed record InvoiceLineDeletedDomainEvent(Guid invoiceLineId) : IDomainEvent;
}
