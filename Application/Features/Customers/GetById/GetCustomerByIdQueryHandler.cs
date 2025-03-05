using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Entities.Customers;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Features.Customers.GetById
{
    public sealed class GetUserByIdQueryHandler(
        IApplicationDbContext context)
        : IQueryHandler<GetCustomerByIdQuery, CustomerResponse>
    {
        public async Task<Result<CustomerResponse>> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            CustomerResponse? customer = await context.Customers
                .Where(customer => customer.Id == request.cutomerId)
                .Select(customer => new CustomerResponse
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    Email = customer.Email,
                    Phone = customer.Phone,
                    Address = customer.Address
                })
                .SingleOrDefaultAsync(cancellationToken);

            if (customer is null)
            {
                return Result.Failure<CustomerResponse>(CustomerError.NotFound(request.cutomerId));
            }

            return customer;
        }
    }
}
