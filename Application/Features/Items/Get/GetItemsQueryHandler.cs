using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Features.Items.Get
{
    internal sealed class GetItemsQueryHandler(
        IApplicationDbContext context)
        : IQueryHandler<GetItemsQuery, List<ItemsRespone>>
    {
        public async Task<Result<List<ItemsRespone>>> Handle(GetItemsQuery request, CancellationToken cancellationToken)
        {
            List<ItemsRespone> items = await context.Items
                .Select(item => new ItemsRespone
                {
                    ItemId = item.Id,
                    Name = item.Name,
                    Barcode = item.Barcode,
                    Descriptions = item.Descriptions,
                    UnitPrice = item.UnitPrice
                })
                .ToListAsync(cancellationToken);

            return items;
        }
    }
}
