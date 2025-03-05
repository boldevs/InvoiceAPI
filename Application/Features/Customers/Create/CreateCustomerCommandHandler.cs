using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Entities.Customers;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Features.Customers.Create
{
    public sealed class CreateCustomerCommandHandler(
        IApplicationDbContext context)
        : ICommandHandler<CreateCustomerCommand, Guid>
    {
        public async Task<Result<Guid>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var existingCustomer = await context.Customers
                .FirstOrDefaultAsync(c => c.Email == request.Email || c.Phone == request.Phone, cancellationToken);

            if (existingCustomer is not null)
            {
                if (existingCustomer.Email == request.Email)
                {
                    return Result.Failure<Guid>(CustomerError.EmailNotUnique);
                }
                if (existingCustomer.Phone == request.Phone)
                {
                    return Result.Failure<Guid>(CustomerError.DuplicatePhoneNumber);
                }
            }

            var customer = new Customer
            {
                Email = request.Email,
                Name = request.Name,
                Phone = request.Phone,
                Address = request.Address
            };

            customer.Raise(new CustomerCreatedDomainEvent(customer.Id));

            context.Customers.Add(customer);

            try
            {
                await context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                return Result.Failure<Guid>(CustomerError.DatabaseError(ex.Message));
            }

            return customer.Id;
        }
    }
}
