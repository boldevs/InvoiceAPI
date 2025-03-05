using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Entities.Customers;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Features.Customers.Delete
{
    public sealed class DeleteCustomerCommandHandler(
        IApplicationDbContext context)
        : ICommandHandler<DeleteCustomerCommand>
    {
        public async Task<Result> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            Customer? customer = await context.Customers
                .SingleOrDefaultAsync(c => c.Id == request.customerId);

            if (customer is null)
                return Result.Failure(CustomerError.NotFound(request.customerId));

            context.Customers.Remove(customer);

            customer.Raise(new CustomerDeletedDomainEvent(customer.Id));

            await context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
