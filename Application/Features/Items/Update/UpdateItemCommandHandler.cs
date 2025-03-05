using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Entities.Items;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Features.Items.Update
{
    internal class UpdateItemCommandHandler(
        IApplicationDbContext context)
        : ICommandHandler<UpdateItemCommand>
    {
        public async Task<Result> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
        {
            var items = await context.Items
                .Where(c => c.Id == request.ItemID || c.Name == request.Name || c.Barcode == request.BarCode)
                .ToListAsync(cancellationToken);

            var existingItem = items.FirstOrDefault(c => c.Id == request.ItemID);

            if (existingItem is null)
                return Result.Failure(ItemError.NotFound(request.ItemID));

            if (items.Any(c => c.Name == request.Name && c.Id != request.ItemID))
                return Result.Failure(ItemError.DuplicateName);

            if (items.Any(c => c.Name == request.Name && c.Id != request.ItemID))
                return Result.Failure(ItemError.DuplicateBarcode);

            existingItem.Name = request.Name;
            existingItem.Barcode = request.BarCode;
            existingItem.Descriptions = request.Descriptions;
            existingItem.UnitPrice = request.UnitPrice;

            existingItem.Raise(new ItemUpdatedDomainEvent(existingItem.Id));

            try
            {
                await context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                return Result.Failure(ItemError.DatabaseError(ex.Message));
            }

            return Result.Success();

        }
    }
}
