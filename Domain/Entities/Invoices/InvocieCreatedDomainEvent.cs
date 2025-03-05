using SharedKernel;

namespace Domain.Entities.Invoices
{
    public sealed record InvocieCreatedDomainEvent(Guid ivnoiceId) : IDomainEvent;
}
