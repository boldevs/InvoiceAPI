using Application.Abstractions.Messaging;

namespace Application.Features.Items.GetById
{
    public sealed record GetItemByIdQuery(Guid itemId) : IQuery<ItemResponse>;
}
