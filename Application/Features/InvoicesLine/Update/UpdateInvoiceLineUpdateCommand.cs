using Application.Abstractions.Messaging;

namespace Application.Features.InvoicesLine.Update
{
    public sealed record UpdateInvoiceLineUpdateCommand(
        Guid Id,
        Guid ItemId,
        int Quantity,
        decimal UnitPrice
    ) : ICommand;
}
