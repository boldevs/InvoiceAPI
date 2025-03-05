using Application.Abstractions.Messaging;

namespace Application.Features.Customers.GetById
{
    public sealed record GetCustomerByIdQuery(Guid cutomerId) : IQuery<CustomerResponse>;
}
