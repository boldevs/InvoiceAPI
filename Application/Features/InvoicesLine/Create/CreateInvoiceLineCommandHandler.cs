using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Entities.InvoiceLines;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Features.InvoicesLine.Create
{
    internal sealed class CreateInvoiceLineCommandHandler(
        IApplicationDbContext context)
        : ICommandHandler<CreateInvoiceLineCommand, Guid>
    {
        public async Task<Result<Guid>> Handle(CreateInvoiceLineCommand request, CancellationToken cancellationToken)
        {
            var invoiceExists = await context.Invoices.AnyAsync(i => i.Id == request.InvoiceId, cancellationToken);

            if (!invoiceExists)
            {
                return Result<Guid>.ValidationFailure(InvoiceLineError.NotFoundIvnoiceId);
            }

            var itemExist = await context.Items.AnyAsync(i => i.Id == request.ItemId, cancellationToken);

            if (!itemExist)
            {
                return Result<Guid>.ValidationFailure(InvoiceLineError.NotFoundItemId);
            }

            var invoiceLine = new InvoiceLine
            {
                InvoiceId = request.InvoiceId,
                ItemId = request.ItemId,
                Quantity = request.Quantity,
                UnitPrice = request.UnitPrice
            };

            invoiceLine.Raise(new InvoiceLineCreatedDomainEvent(invoiceLine.Id));

            context.InvoiceLines.Add(invoiceLine);

            try
            {
                await context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                return Result.Failure<Guid>(InvoiceLineError.DatabaseError(ex.Message));
            }

            return invoiceLine.Id;
        }
    }
}
