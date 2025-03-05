using Domain.Entities.Customers;
using Domain.Entities.InvoiceLines;
using Domain.Entities.Invoices;
using Domain.Entities.Items;
using Microsoft.EntityFrameworkCore;

namespace Application.Abstractions.Data
{
    public interface IApplicationDbContext
    {
        DbSet<Customer> Customers { get; set; }
        DbSet<Item> Items { get; set; }
        DbSet<Invoice> Invoices { get; set; }
        DbSet<InvoiceLine> InvoiceLines { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }

}
