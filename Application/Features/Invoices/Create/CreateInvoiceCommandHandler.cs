using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Entities.Customers;
using Domain.Entities.Invoices;
using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Features.Invoices.Create
{
    internal sealed class CreateInvoiceCommandHandler(
        IApplicationDbContext context,
        IUserService userService)
        : ICommandHandler<CreateInvoiceCommand, Guid>
    {
        public async Task<Result<Guid>> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
        {
            var userId = await userService.UserExistsByIdAsync(request.UserId);

            if (!userId)
            {
                return Result.Failure<Guid>(UserError.NotFound);
            }

            var existingCustomer = await context.Customers
                .FirstOrDefaultAsync(c => c.Id == request.CustomerId, cancellationToken);

            if (existingCustomer is null)
            {
                return Result.Failure<Guid>(CustomerError.NotFound(request.CustomerId));
            }

            var existInvoiceNumber = await context.Invoices
                .FirstOrDefaultAsync(i => i.InvoiceNumber == request.InvoiceNumber, cancellationToken);

            if (existInvoiceNumber is not null)
            {
                return Result.Failure<Guid>(InvoiceError.InvoiceNumberNotUnique);
            }

            var invoice = new Invoice
            {
                InvoiceNumber = request.InvoiceNumber,
                UserId = request.UserId,
                CustomerId = request.CustomerId,
                DueDate = request.DueDate,
                IssuedDate = request.IssuedDate,
                TotalAmount = request.TotalAmount
            };

            invoice.Raise(new InvocieCreatedDomainEvent(invoice.Id));

            context.Invoices.Add(invoice);

            try
            {
                await context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                return Result.Failure<Guid>(InvoiceError.DatabaseError(ex.Message));
            }

            return invoice.Id;
        }
    }
}
