using SharedKernel;

namespace Domain.Entities.Customers
{
    public sealed record CustomerCreatedDomainEvent(Guid customerId) : IDomainEvent;
}
