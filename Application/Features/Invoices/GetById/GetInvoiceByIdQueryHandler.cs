using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Entities.Invoices;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Features.Invoices.GetById
{
    internal sealed class GetInvoiceByIdQueryHandler(
        IApplicationDbContext context,
        IUserService userService)
        : IQueryHandler<GetInvoiceByIdQuery, InvoiceResponse>
    {
        public async Task<Result<InvoiceResponse>> Handle(GetInvoiceByIdQuery request, CancellationToken cancellationToken)
        {
            var invoice = await context.Invoices.
                FirstOrDefaultAsync(x => x.Id == request.invoiceId, cancellationToken);

            if (invoice is null)
                return Result.Failure<InvoiceResponse>(InvoiceError.NotFound(request.invoiceId));

            var user = await userService.GetUserByIdAsync(invoice.UserId);
            var userName = user != null ? $"{user.FirstName} {user.LastName}" : "Unknown";

            var customer = await context.Customers
                .FirstOrDefaultAsync(c => c.Id == invoice.CustomerId, cancellationToken);
            var customerName = customer != null ? customer.Name : "Unknown";

            var invoiceResponse = new InvoiceResponse
            {
                Id = invoice.Id,
                InvoiceNumber = invoice.InvoiceNumber,
                UserId = invoice.UserId,
                UserName = userName,
                CustomerId = invoice.CustomerId,
                CustomerName = customerName,
                DueDate = invoice.DueDate,
                IssuedDate = invoice.IssuedDate,
                TotalAmount = invoice.TotalAmount
            };

            return Result.Success(invoiceResponse);
        }
    }
}
