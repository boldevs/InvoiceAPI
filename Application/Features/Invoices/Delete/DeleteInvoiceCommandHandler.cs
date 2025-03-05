using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Entities.Customers;
using Domain.Entities.Invoices;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Features.Invoices.Delete
{
    internal class DeleteInvoiceCommandHandler(
        IApplicationDbContext context)
        : ICommandHandler<DeleteInvoiceCommand>
    {
        public async Task<Result> Handle(DeleteInvoiceCommand request, CancellationToken cancellationToken)
        {
            Invoice? invoice = await context.Invoices
                .SingleOrDefaultAsync(c => c.Id == request.invoiceId);

            if (invoice is null)
                return Result.Failure(InvoiceError.NotFound(request.invoiceId));

            context.Invoices.Remove(invoice);

            invoice.Raise(new CustomerCreatedDomainEvent(invoice.Id));

            await context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
