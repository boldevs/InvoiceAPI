using Domain.Entities.InvoiceLines;
using Domain.Entities.Invoices;
using Domain.Entities.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configuration.InvoiceLines
{
    internal sealed class InvoiceLineConfiguration : IEntityTypeConfiguration<InvoiceLine>
    {
        public void Configure(EntityTypeBuilder<InvoiceLine> builder)
        {
            // Primary Key
            builder.HasKey(x => x.Id);

            // Use NEWSEQUENTIALID() for better indexing performance
            builder.Property(x => x.Id)
                .HasDefaultValueSql("NEWSEQUENTIALID()");

            // CreatedAt - Default to current UTC time
            builder.Property(x => x.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd();

            // UpdatedAt - Automatically update on modification
            builder.Property(x => x.UpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAddOrUpdate();

            // InvoiceId - Required Foreign Key
            builder.Property(x => x.InvoiceId)
                .IsRequired();

            // ItemId - Required Foreign Key
            builder.Property(x => x.ItemId)
                .IsRequired();

            // Quantity - Required, Default to 1
            builder.Property(x => x.Quantity)
                .IsRequired()
                .HasDefaultValue(1);

            // UnitPrice - Required, Default to 0.00
            builder.Property(x => x.UnitPrice)
                .IsRequired()
                .HasColumnType("DECIMAL(10,2)")
                .HasDefaultValue(0.00);

            // SubTotalTotalPrice - Auto-Generated (Quantity * UnitPrice)
            builder.Property(x => x.SubTotalTotalPrice)
                .HasColumnType("DECIMAL(10,2)")
                .HasComputedColumnSql("[Quantity] * [UnitPrice]"); // Auto-calculated field

            // Relationships
            builder.HasOne<Invoice>()
                .WithMany()
                .HasForeignKey(x => x.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Item>()
                .WithMany()
                .HasForeignKey(x => x.ItemId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent item deletion if used in invoices
        }
    }
}
