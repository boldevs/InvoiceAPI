using SharedKernel;

namespace Domain.Entities.InvoiceLines
{
    public sealed record InvoiceLineUpdatedDomainEvent(Guid invoiceLineId) : IDomainEvent;
}
