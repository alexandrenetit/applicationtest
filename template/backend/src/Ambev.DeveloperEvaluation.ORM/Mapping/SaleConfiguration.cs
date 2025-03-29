using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.ToTable("Sales");

        // Primary Key
        builder.HasKey(s => s.Id);

        // Properties
        builder.Property(s => s.SaleNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(s => s.SaleDate)
            .IsRequired();

        builder.Property(s => s.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        // Owned Money Type for TotalAmount
        builder.OwnsOne(s => s.TotalAmount, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("TotalAmount")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            money.Property(m => m.Currency)
                .HasColumnName("Currency")
                .HasMaxLength(3)
                .IsRequired();

            money.Ignore(m => m.Symbol);
            money.Ignore(m => m.Formatted);
        });

        // Relationships
        builder.HasOne(s => s.Customer)
            .WithMany()
            .HasForeignKey(s => s.Id)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.Branch)
            .WithMany()
            .HasForeignKey(s => s.Id)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        // Navigation property for Items
        builder.HasMany(s => s.Items)
            .WithOne()
            .HasForeignKey("Id")
            .OnDelete(DeleteBehavior.Cascade);
    }
}