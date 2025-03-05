using Application.Abstractions.Messaging;

namespace Application.Features.Invoices.Create
{
    public sealed class CreateInvoiceCommand : ICommand<Guid>
    {
        public string InvoiceNumber { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime IssuedDate { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
