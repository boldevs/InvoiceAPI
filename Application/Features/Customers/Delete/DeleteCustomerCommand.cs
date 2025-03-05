using Application.Abstractions.Messaging;

namespace Application.Features.Customers.Delete
{
    public sealed record DeleteCustomerCommand(Guid customerId) : ICommand;
}
