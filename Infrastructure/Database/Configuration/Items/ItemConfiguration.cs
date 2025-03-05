using Domain.Entities.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configuration.Items
{
    internal sealed class ItemConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.HasKey(p => p.Id);

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

            // Barcode - Required, Unique (For inventory tracking)
            builder.Property(x => x.Barcode)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnType("VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS"); // Case-insensitive

            builder.HasIndex(x => x.Barcode)
                .IsUnique();

            // Name - Required and Indexed for faster lookup
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(255);
            builder.HasIndex(x => x.Name);

            // Descriptions - Optional, Allows Null
            builder.Property(x => x.Descriptions)
                .IsRequired(false)
                .HasMaxLength(500);

            // UnitPrice - Required, Default to 0.00
            builder.Property(x => x.UnitPrice)
                .IsRequired()
                .HasColumnType("DECIMAL(10,2)")
                .HasDefaultValue(0.00);
        }
    }
}
