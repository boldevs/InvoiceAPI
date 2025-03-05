using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Features.Customers.Get
{
    public sealed class GetCustomersQueryHandler(
        IApplicationDbContext context)
        : IQueryHandler<GetCustomersQuery, List<CustomerResponse>>
    {
        public async Task<Result<List<CustomerResponse>>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
        {
            List<CustomerResponse> customers = await context.Customers
                .Select(customer => new CustomerResponse
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    Email = customer.Email,
                    Phone = customer.Phone,
                    Address = customer.Address
                })
                .ToListAsync(cancellationToken);

            return customers;
        }
    }
}
