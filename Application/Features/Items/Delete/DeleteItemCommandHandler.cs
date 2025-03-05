using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Entities.Items;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Features.Items.Delete
{
    internal sealed class DeleteItemCommandHandler(
        IApplicationDbContext context)
        : ICommandHandler<DeleteItemCommand>
    {
        public async Task<Result> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
        {
            Item? item = await context.Items
                .SingleOrDefaultAsync(i => i.Id == request.itemId);

            if (item is null)
                return Result.Failure(ItemError.NotFound(request.itemId));

            context.Items.Remove(item);

            item.Raise(new ItemDeletedDomainEvent(item.Id));

            await context.SaveChangesAsync(cancellationToken);

            return Result.Success();

        }
    }
}
