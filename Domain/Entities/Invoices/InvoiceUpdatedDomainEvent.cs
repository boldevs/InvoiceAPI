using SharedKernel;

namespace Domain.Entities.Invoices
{
    public sealed record InvoiceUpdatedDomainEvent(Guid invoiceId) : IDomainEvent;
}
