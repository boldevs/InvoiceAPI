using SharedKernel;

namespace Domain.Entities.InvoiceLines
{
    public sealed record InvoiceLineCreatedDomainEvent(Guid ivnoiceLineId) : IDomainEvent;
}
