using Application.Abstractions.Messaging;

namespace Application.Features.Customers.Update
{
    public sealed record UpdateCustomerCommand(
        Guid CustomerId,
        string Email,
        string Name,
        string Phone,
        string Address
    ) : ICommand;
}
