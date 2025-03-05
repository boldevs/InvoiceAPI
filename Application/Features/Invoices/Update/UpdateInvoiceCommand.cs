using Application.Abstractions.Messaging;

namespace Application.Features.Invoices.Update
{
    public sealed record UpdateInvoiceCommand(
        Guid Id,
        Guid CustomerId,
        Guid UserId,
        string InvoiceNumber,
        DateTime DueDate,
        DateTime IssuedDate,
        decimal TotalAmount
    ) : ICommand;
}
