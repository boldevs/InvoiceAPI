using SharedKernel;

namespace Domain.Entities.Items
{
    public sealed record ItemUpdatedDomainEvent(Guid itemId) : IDomainEvent;
}
