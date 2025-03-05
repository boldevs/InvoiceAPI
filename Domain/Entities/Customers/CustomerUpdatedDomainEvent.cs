using SharedKernel;

namespace Domain.Entities.Customers
{
    public sealed record CustomerUpdatedDomainEvent(Guid customerId) : IDomainEvent;
}
