using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Entities.Invoices;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Features.Invoices.Update
{
    internal class UpdateInvoiceCommandHandler(
        IApplicationDbContext context)
        : ICommandHandler<UpdateInvoiceCommand>
    {
        public async Task<Result> Handle(UpdateInvoiceCommand request, CancellationToken cancellationToken)
        {
            var invoices = await context.Invoices
                .Where(i => i.Id == request.Id || i.InvoiceNumber == request.InvoiceNumber)
                .ToListAsync(cancellationToken);

            var existingInvoice = invoices.FirstOrDefault(i => i.Id == request.Id);

            if (existingInvoice != null)
                return Result.Failure(InvoiceError.NotFound(request.Id));

            if (invoices.Any(i => i.InvoiceNumber == request.InvoiceNumber && i.Id != request.Id))
                return Result.Failure(InvoiceError.InvoiceNumberNotUnique);

            existingInvoice.CustomerId = request.CustomerId;
            existingInvoice.UserId = request.UserId;
            existingInvoice.InvoiceNumber = request.InvoiceNumber;
            existingInvoice.DueDate = request.DueDate;
            existingInvoice.IssuedDate = request.IssuedDate;
            existingInvoice.TotalAmount = request.TotalAmount;

            existingInvoice.Raise(new InvoiceUpdatedDomainEvent(existingInvoice.Id));

            try
            {
                await context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                return Result.Failure(InvoiceError.DatabaseError(ex.Message));
            }

            return Result.Success();
        }
    }
}
