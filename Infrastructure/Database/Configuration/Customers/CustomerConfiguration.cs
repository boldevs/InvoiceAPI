using Domain.Entities.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configuration.Customers
{
    internal sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            // Primary Key
            builder.HasKey(x => x.Id);

            // Use NEWSEQUENTIALID() instead of NEWID() for performance
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

            // Email - Required, Unique (Case-Insensitive)
            builder.Property(x => x.Email)
                 .IsRequired()
                 .HasMaxLength(255)
                 .HasColumnType("VARCHAR(255) COLLATE SQL_Latin1_General_CP1_CI_AS"); // Case-insensitive

            builder.HasIndex(x => x.Email)
                .IsUnique();

            // Name - Required and Indexed for faster lookup
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);
            builder.HasIndex(x => x.Name);

            // Phone - Required with validation
            builder.Property(x => x.Phone)
                .IsRequired()
                .HasMaxLength(20)
                .HasAnnotation("RegularExpression", @"^\+?[1-9]\d{1,14}$"); // E.164 format

            // Address - Optional, allowing flexibility
            builder.Property(x => x.Address)
                .IsRequired(false)
                .HasMaxLength(300);
        }
    }
}
