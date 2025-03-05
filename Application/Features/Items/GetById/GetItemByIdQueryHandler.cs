using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Entities.Items;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Features.Items.GetById
{
    internal class GetItemByIdQueryHandler(
        IApplicationDbContext context)
        : IQueryHandler<GetItemByIdQuery, ItemResponse>
    {
        public async Task<Result<ItemResponse>> Handle(GetItemByIdQuery request, CancellationToken cancellationToken)
        {
            ItemResponse? item = await context.Items
                .Where(item => item.Id == request.itemId)
                .Select(item => new ItemResponse
                {
                    ItemId = item.Id,
                    Name = item.Name,
                    Barcode = item.Barcode,
                    Descriptions = item.Descriptions,
                    UnitPrice = item.UnitPrice
                })
                .SingleOrDefaultAsync(cancellationToken);

            return item is null
                ? Result.Failure<ItemResponse>(ItemError.NotFound(request.itemId))
                : item;
        }
    }
}
