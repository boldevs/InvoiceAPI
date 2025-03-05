using SharedKernel;

namespace Domain.Entities.InvoiceLines
{
    public sealed class InvoiceLine : BaseEntity
    {
        public Guid InvoiceId { get; set; }
        public Guid ItemId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotalTotalPrice { get; set; }
    }
}
