using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Entities.Customers;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Features.Customers.Update
{
    public sealed class UpdateCustomerCommandHandler(
        IApplicationDbContext context)
        : ICommandHandler<UpdateCustomerCommand>
    {
        public async Task<Result> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {

            var customers = await context.Customers
                .Where(c => c.Id == request.CustomerId || c.Email == request.Email || c.Phone == request.Phone)
                .ToListAsync(cancellationToken);

            var existingCustomer = customers.FirstOrDefault(c => c.Id == request.CustomerId);

            if (existingCustomer is null)
                return Result.Failure(CustomerError.NotFound(request.CustomerId));

            if (customers.Any(c => c.Email == request.Email && c.Id != request.CustomerId))
                return Result.Failure(CustomerError.EmailNotUnique);

            if (customers.Any(c => c.Phone == request.Phone && c.Id != request.CustomerId))
                return Result.Failure(CustomerError.DuplicatePhoneNumber);

            existingCustomer.Email = request.Email;
            existingCustomer.Name = request.Name;
            existingCustomer.Phone = request.Phone;
            existingCustomer.Address = request.Address;

            existingCustomer.Raise(new CustomerUpdatedDomainEvent(existingCustomer.Id));

            try
            {
                await context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                return Result.Failure(CustomerError.DatabaseError(ex.Message));
            }

            return Result.Success();
        }
    }
}
