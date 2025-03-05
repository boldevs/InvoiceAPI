using Application.Abstractions.Messaging;

namespace Application.Features.Items.Delete
{
    public sealed record DeleteItemCommand(Guid itemId) : ICommand;
}
