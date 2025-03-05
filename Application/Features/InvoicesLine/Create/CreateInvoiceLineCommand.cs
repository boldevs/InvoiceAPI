using Application.Abstractions.Messaging;

namespace Application.Features.InvoicesLine.Create
{
    public sealed class CreateInvoiceLineCommand : ICommand<Guid>
    {
        public Guid InvoiceId { get; set; }
        public Guid ItemId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
