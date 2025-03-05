using SharedKernel;

namespace Domain.Entities.Items
{
    public sealed record ItemDeletedDomainEvent(Guid itemId) : IDomainEvent;
}
