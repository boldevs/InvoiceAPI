using Application.Abstractions.Messaging;

namespace Application.Features.Customers.Get
{
    public sealed class GetCustomersQuery : IQuery<List<CustomerResponse>>;
}
