using Application.Abstractions.Messaging;

namespace Application.Features.Items.Get
{
    public sealed class GetItemsQuery : IQuery<List<ItemsRespone>>;
}

