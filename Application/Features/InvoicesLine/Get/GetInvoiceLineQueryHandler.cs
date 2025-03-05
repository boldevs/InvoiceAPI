using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Features.InvoicesLine.Get
{
    internal sealed class GetInvoiceLineQueryHandler(
        IApplicationDbContext context)
        : IQueryHandler<GetInvoiceLineQuery, PagedResult<InvoiceLineReponse>>
    {
        public async Task<Result<PagedResult<InvoiceLineReponse>>> Handle(GetInvoiceLineQuery request, CancellationToken cancellationToken)
        {
            var invoiceLinesQuery = context.InvoiceLines.AsNoTracking(); // Read-only optimization

            if (request.InvoiceId.HasValue)
            {
                invoiceLinesQuery = invoiceLinesQuery.Where(i => i.InvoiceId == request.InvoiceId.Value);
            }

            if (request.ItemId.HasValue)
            {
                invoiceLinesQuery = invoiceLinesQuery.Where(i => i.ItemId == request.ItemId.Value);
            }

            var totalCount = await invoiceLinesQuery.CountAsync(cancellationToken);

            var invoiceLines = await invoiceLinesQuery
                .OrderBy(i => i.InvoiceId)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(i => new
                {
                    i.InvoiceId,
                    i.ItemId,
                    i.Quantity,
                    i.UnitPrice
                }) // Projection before materialization
                .ToListAsync(cancellationToken);

            // Get unique item IDs
            var itemIds = invoiceLines.Select(i => i.ItemId).Distinct().ToList();

            // Fetch item details
            var items = await context.Items
                .Where(i => itemIds.Contains(i.Id))
                .ToDictionaryAsync(i => i.Id, cancellationToken);

            var invoiceLineResponse = invoiceLines.Select(i => new InvoiceLineReponse
            {
                InvoiceId = i.InvoiceId,
                ItemId = i.ItemId,
                ItemName = items.TryGetValue(i.ItemId, out var item) ? item.Name : "Unknown",
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList();

            var pageResult = new PagedResult<InvoiceLineReponse>(invoiceLineResponse, totalCount, request.PageNumber, request.PageSize);

            return Result.Success(pageResult);
        }
    }
}
