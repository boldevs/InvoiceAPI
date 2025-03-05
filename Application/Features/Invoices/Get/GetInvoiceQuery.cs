using Application.Abstractions.Messaging;
using SharedKernel;

namespace Application.Features.Invoices.Get
{
    public sealed class GetInvoiceQuery : IQuery<PagedResult<InvoicesResponse>>
    {
        public Guid? UserId { get; set; }
        public Guid? CustomerId { get; set; }
        public string? InvoiceNumber { get; set; }

        // Pagination
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public GetInvoiceQuery(
            Guid? userId = null,
            Guid? customerId = null,
            string? invoiceNumber = null,
            int pageNumber = 1,
            int pageSize = 10)
        {
            UserId = userId;
            CustomerId = customerId;
            InvoiceNumber = invoiceNumber;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
