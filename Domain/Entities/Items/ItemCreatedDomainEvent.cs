using SharedKernel;

namespace Domain.Entities.Items
{
    public sealed record ItemCreatedDomainEvent(Guid itemId) : IDomainEvent;
}
