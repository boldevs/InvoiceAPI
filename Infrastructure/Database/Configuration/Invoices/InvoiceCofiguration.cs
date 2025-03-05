using Domain.Entities.Customers;
using Domain.Entities.Invoices;
using Infrastructure.AspUserSecurity.Identity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configuration.Invoices
{
    internal sealed class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            // Primary Key
            builder.HasKey(x => x.Id);

            // Use NEWSEQUENTIALID() for better performance on indexing
            builder.Property(x => x.Id)
                .HasDefaultValueSql("NEWSEQUENTIALID()");

            // CreatedAt - Default to current UTC time
            builder.Property(x => x.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd();

            // UpdatedAt - Automatically update on modification
            builder.Property(x => x.UpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()") // Default value
                .ValueGeneratedOnAddOrUpdate(); // Update when modified

            // InvoiceNumber - Required, Unique
            builder.Property(x => x.InvoiceNumber)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnType("VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS"); // Case-insensitive

            builder.HasIndex(x => x.InvoiceNumber)
                .IsUnique();

            // UserId - Required, Foreign Key to Users
            builder.Property(x => x.UserId)
                .IsRequired();

            // CustomerId - Required, Foreign Key to Customers
            builder.Property(x => x.CustomerId)
                .IsRequired();

            // IssuedDate - Required
            builder.Property(x => x.IssuedDate)
                .IsRequired();

            // DueDate - Required
            builder.Property(x => x.DueDate)
                .IsRequired();

            // TotalAmount - Required, Default to 0.00
            builder.Property(x => x.TotalAmount)
                .IsRequired()
                .HasColumnType("DECIMAL(10,2)")
                .HasDefaultValue(0.00);

            // IsPaid - Required, Default to False
            builder.Property(x => x.IsPaid)
                .IsRequired()
                .HasDefaultValue(false);

            // Relationships
            builder.HasOne<Customer>()
                .WithMany()
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
