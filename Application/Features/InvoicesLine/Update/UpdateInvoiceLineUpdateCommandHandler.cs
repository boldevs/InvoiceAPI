using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Entities.InvoiceLines;
using Domain.Entities.Invoices;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Features.InvoicesLine.Update
{
    internal sealed class UpdateInvoiceLineUpdateCommandHandler(
        IApplicationDbContext context)
        : ICommandHandler<UpdateInvoiceLineUpdateCommand>
    {
        public async Task<Result> Handle(UpdateInvoiceLineUpdateCommand request, CancellationToken cancellationToken)
        {
            var invoiceLine = await context.InvoiceLines
                .FirstOrDefaultAsync(il => il.Id == request.Id, cancellationToken);

            if (invoiceLine == null)
            {
                return Result.Failure(InvoiceLineError.NotFound(request.Id));
            }

            var itemExists = await context.Items.AnyAsync(i => i.Id == request.ItemId, cancellationToken);
            if (!itemExists)
            {
                return Result.Failure(InvoiceLineError.NotFoundItemId);
            }

            invoiceLine.ItemId = request.ItemId;
            invoiceLine.Quantity = request.Quantity;
            invoiceLine.UnitPrice = request.UnitPrice;

            invoiceLine.Raise(new InvoiceLineUpdatedDomainEvent(invoiceLine.Id));

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
