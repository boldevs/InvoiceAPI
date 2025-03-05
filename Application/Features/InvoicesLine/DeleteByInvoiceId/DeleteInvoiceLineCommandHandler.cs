using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Features.InvoicesLine.DeleteByInvoiceId;
using Domain.Entities.InvoiceLines;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Features.InvoicesLine.DelDeleteByInvoiceIdeteById
{
    internal sealed class DeleteInvoiceLineCommandHandler(
        IApplicationDbContext context)
        : ICommandHandler<DeleteInvoiceLineCommand>
    {
        public async Task<Result> Handle(DeleteInvoiceLineCommand request, CancellationToken cancellationToken)
        {
            var invoiceLines = await context.InvoiceLines
                .Where(c => c.InvoiceId == request.invoiceId)
                .ToListAsync(cancellationToken);

            if (!invoiceLines.Any())
                return Result.Failure(InvoiceLineError.NotFoundIvnoiceId);

            context.InvoiceLines.RemoveRange(invoiceLines);

            foreach (var invoiceLine in invoiceLines)
            {
                invoiceLine.Raise(new InvoiceLineDeletedDomainEvent(invoiceLine.InvoiceId));
            }

            await context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
