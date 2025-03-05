using Application.Abstractions.Messaging;

namespace Application.Features.Items.Update
{
    public sealed record UpdateItemCommand(
        Guid ItemID,
        string BarCode,
        string Name,
        string Descriptions,
        decimal UnitPrice) : ICommand;
}
