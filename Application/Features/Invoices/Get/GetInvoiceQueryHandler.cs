using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Features.Invoices.Get
{
    internal sealed class GetInvoiceQueryHandler(
        IApplicationDbContext context,
        IUserService userService)
        : IQueryHandler<GetInvoiceQuery, PagedResult<InvoicesResponse>>
    {
        public async Task<Result<PagedResult<InvoicesResponse>>> Handle(GetInvoiceQuery request, CancellationToken cancellationToken)
        {
            var invoicesQuery = context.Invoices.AsNoTracking(); // Avoid tracking to optimize read operations.

            if (request.UserId.HasValue)
            {
                invoicesQuery = invoicesQuery.Where(i => i.UserId == request.UserId.Value);
            }

            if (request.CustomerId.HasValue)
            {
                invoicesQuery = invoicesQuery.Where(i => i.CustomerId == request.CustomerId.Value);
            }

            if (!string.IsNullOrEmpty(request.InvoiceNumber))
            {
                invoicesQuery = invoicesQuery.Where(i => i.InvoiceNumber.Contains(request.InvoiceNumber));
            }

            var totalCount = await invoicesQuery.CountAsync(cancellationToken);

            var invoices = await invoicesQuery
                .OrderByDescending(i => i.IssuedDate)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(i => new
                {
                    i.Id,
                    i.InvoiceNumber,
                    i.UserId,
                    i.CustomerId,
                    i.DueDate,
                    i.IssuedDate,
                    i.TotalAmount
                }) // Projection before materialization
                .ToListAsync(cancellationToken);

            var userIds = invoices.Select(i => i.UserId).Distinct().ToList();
            var customerIds = invoices.Select(i => i.CustomerId).Distinct().ToList();

            var usersList = await userService.GetUsersByIdsAsync(userIds);
            var usersDictionary = usersList.ToDictionary(u => Guid.Parse(u.Id.ToString()));

            var customers = await context.Customers
                .Where(c => customerIds.Contains(c.Id))
                .ToDictionaryAsync(c => c.Id, cancellationToken);

            var invoicesResponse = invoices.Select(i => new InvoicesResponse
            {
                Id = i.Id,
                InvoiceNumber = i.InvoiceNumber,
                UserId = i.UserId,
                UserName = usersDictionary.TryGetValue(i.UserId, out var user) ? $"{user.FirstName} {user.LastName}" : "Unknown",
                CustomerId = i.CustomerId,
                CustomerName = customers.TryGetValue(i.CustomerId, out var customer) ? customer.Name : "Unknown",
                DueDate = i.DueDate,
                IssuedDate = i.IssuedDate,
                TotalAmount = i.TotalAmount
            }).ToList();

            var pagedResult = new PagedResult<InvoicesResponse>(invoicesResponse, totalCount, request.PageNumber, request.PageSize);
            return Result.Success(pagedResult);
        }
    }
}
