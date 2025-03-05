using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Entities.InvoiceLines;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Features.InvoicesLine.GetByInvoiceId
{
    internal sealed class GetInvoiceLineByIdQueryHandler(
            IApplicationDbContext context)
            : IQueryHandler<GetInvoiceLineByIdQuery, List<InvoiceLineResponse>>
    {
        public async Task<Result<List<InvoiceLineResponse>>> Handle(GetInvoiceLineByIdQuery request, CancellationToken cancellationToken)
        {
            var invoiceLines = await context.InvoiceLines.
                Where(x => x.InvoiceId == request.invoiceId).ToListAsync(cancellationToken);

            if (invoiceLines is null)
                return Result.Failure<List<InvoiceLineResponse>>(InvoiceLineError.NotFound(request.invoiceId));

            var itemIds = invoiceLines.Select(x => x.ItemId).Distinct().ToList();
            var items = await context.Items
                .Where(c => itemIds.Contains(c.Id))
                .ToDictionaryAsync(c => c.Id, c => c.Name, cancellationToken);

            var invoiceLineResponses = invoiceLines.Select(invoiceLine => new InvoiceLineResponse
            {
                InvoiceLineId = invoiceLine.Id,
                InvoiceId = invoiceLine.InvoiceId,
                ItemId = invoiceLine.ItemId,
                ItemName = items.TryGetValue(invoiceLine.ItemId, out var name) ? name : "Unknown",
                Quantity = invoiceLine.Quantity,
                SubTotalTotalPrice = invoiceLine.SubTotalTotalPrice,
                UnitPrice = invoiceLine.UnitPrice
            }).ToList();

            return Result.Success(invoiceLineResponses);
        }
    }
}
