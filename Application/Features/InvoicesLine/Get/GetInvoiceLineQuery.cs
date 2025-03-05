using Application.Abstractions.Messaging;
using SharedKernel;

namespace Application.Features.InvoicesLine.Get
{
    public sealed class GetInvoiceLineQuery : IQuery<PagedResult<InvoiceLineReponse>>
    {
        public Guid? InvoiceId { get; set; }
        public Guid? ItemId { get; set; }

        // Pagination
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public GetInvoiceLineQuery(
            Guid? invoiceId = null,
            Guid? itemId = null,
            int pageNumber = 1,
            int pageSize = 10)
        {
            InvoiceId = invoiceId;
            ItemId = itemId;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
