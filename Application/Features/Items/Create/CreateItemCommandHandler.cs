using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Entities.Items;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Features.Items.Create
{
    internal class CreateItemCommandHandler(
        IApplicationDbContext context)
        : ICommandHandler<CreateItemCommand, Guid>
    {
        public async Task<Result<Guid>> Handle(CreateItemCommand request, CancellationToken cancellationToken)
        {
            var items = await context.Items
                .Where(c => c.Name == request.Name || c.Barcode == request.Barcode)
                .ToListAsync(cancellationToken);

            if (items.Any(c => c.Name == request.Name))
                return Result.Failure<Guid>(ItemError.DuplicateName);

            if (items.Any(c => c.Barcode == request.Barcode))
                return Result.Failure<Guid>(ItemError.DuplicateBarcode);

            var item = new Item
            {
                Barcode = request.Barcode,
                Name = request.Name,
                Descriptions = request.Descriptions,
                UnitPrice = request.UnitPrice
            };

            item.Raise(new ItemCreatedDomainEvent(item.Id));

            context.Items.Add(item);

            try
            {
                await context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                return Result.Failure<Guid>(ItemError.DatabaseError(ex.Message));
            }

            return item.Id;

        }
    }
}
