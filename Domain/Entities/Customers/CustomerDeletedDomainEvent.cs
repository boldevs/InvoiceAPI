using SharedKernel;

namespace Domain.Entities.Customers
{
    public sealed record CustomerDeletedDomainEvent(Guid TodoItemId) : IDomainEvent;
}
