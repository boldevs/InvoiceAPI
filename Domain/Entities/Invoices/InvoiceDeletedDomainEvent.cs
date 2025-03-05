using SharedKernel;

namespace Domain.Entities.Invoices
{
    public sealed record InvoiceDeletedDomainEvent(Guid invoiceId) : IDomainEvent;
}
