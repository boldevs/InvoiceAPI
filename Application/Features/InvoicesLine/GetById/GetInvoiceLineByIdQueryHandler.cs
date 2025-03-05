using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Entities.InvoiceLines;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Features.InvoicesLine.GetById
{
    internal sealed class GetInvoiceLineByIdQueryHandler(
        IApplicationDbContext context)
        : IQueryHandler<GetInvoiceLineByIdQuery, InvoiceLineResponse>
    {
        public async Task<Result<InvoiceLineResponse>> Handle(GetInvoiceLineByIdQuery request, CancellationToken cancellationToken)
        {
            var invoiceLine = await context.InvoiceLines.
                FirstOrDefaultAsync(x => x.Id == request.invoiceLineId, cancellationToken);

            if (invoiceLine is null)
                return Result.Failure<InvoiceLineResponse>(InvoiceLineError.NotFound(request.invoiceLineId));

            var item = await context.Items
                .FirstOrDefaultAsync(c => c.Id == invoiceLine.ItemId, cancellationToken);
            var itemName = item != null ? item.Name : "Unknown";

            var invoiceLineResponse = new InvoiceLineResponse
            {
                InvoiceLineId = invoiceLine.Id,
                InvoiceId = invoiceLine.Id,
                ItemId = invoiceLine.ItemId,
                ItemName = itemName,
                Quantity = invoiceLine.Quantity,
                SubTotalTotalPrice = invoiceLine.SubTotalTotalPrice,
                UnitPrice = invoiceLine.UnitPrice
            };

            return Result.Success(invoiceLineResponse);
        }
    }
}
