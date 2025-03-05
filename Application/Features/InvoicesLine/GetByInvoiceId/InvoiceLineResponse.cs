namespace Application.Features.InvoicesLine.GetByInvoiceId
{
    public sealed class InvoiceLineResponse
    {
        public Guid InvoiceLineId { get; set; }
        public Guid InvoiceId { get; set; }
        public Guid ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotalTotalPrice { get; set; }
    }
}
