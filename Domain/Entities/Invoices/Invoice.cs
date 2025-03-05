using SharedKernel;

namespace Domain.Entities.Invoices
{
    public sealed class Invoice : BaseEntity
    {
        public string InvoiceNumber { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime IssuedDate { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsPaid { get; set; }
    }
}
