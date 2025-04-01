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

        builder.Property(s => s.CustomerId)
            .IsRequired()
            .HasColumnName("CustomerId");

        builder.Property(s => s.BranchId)
            .IsRequired()
            .HasColumnName("BranchId");

        // Owned Money Type for TotalAmount
        builder.OwnsOne(si => si.TotalAmount, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("TotalAmount")
                .HasColumnType("decimal(18,2)")
                .IsRequired();  // Add this
            money.Property(m => m.Currency)
                .HasColumnName("TotalAmountCurrency")
                .HasMaxLength(3)
                .IsRequired();  // Add this
            money.Ignore(m => m.Symbol);
            money.Ignore(m => m.Formatted);
        });

        // Relationships - Corrected
        builder.HasOne(s => s.Customer)
           .WithMany()
           .HasForeignKey(s => s.CustomerId)  // Correct foreign key
           .IsRequired()
           .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.Branch)
            .WithMany()
            .HasForeignKey(s => s.BranchId)  // Correct foreign key
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        // Navigation property for Items - Corrected
        builder.HasMany(s => s.Items)
            .WithOne()
            .HasForeignKey(si => si.SaleId)  // Should be SaleId in SaleItem
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(s => s.SaleNumber).IsUnique();
        builder.HasIndex(s => s.SaleDate);
        builder.HasIndex(s => s.CustomerId);
        builder.HasIndex(s => s.BranchId);
    }
}