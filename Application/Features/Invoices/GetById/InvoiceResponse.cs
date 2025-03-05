namespace Application.Features.Invoices.GetById
{
    public sealed class InvoiceResponse
    {
        public Guid Id { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public Guid CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime IssuedDate { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
