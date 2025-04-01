using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class SaleItemConfiguration : IEntityTypeConfiguration<SaleItem>
{
    public void Configure(EntityTypeBuilder<SaleItem> builder)
    {
        builder.ToTable("SaleItems");

        // Primary Key
        builder.HasKey(si => si.Id);

        // Properties
        builder.Property(si => si.Quantity)
            .IsRequired();

        builder.Property(si => si.Discount)
            .HasColumnType("decimal(5,4)")
            .IsRequired();

        builder.Property(si => si.SaleId)
            .IsRequired();

        builder.Property(si => si.ProductId)
            .IsRequired();

        // Relationships
        builder.HasOne(si => si.Product)
            .WithMany()
            .HasForeignKey(si => si.ProductId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Sale>()
            .WithMany(s => s.Items)
            .HasForeignKey(si => si.SaleId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // Configure Money as owned entity for UnitPrice
        builder.OwnsOne(si => si.UnitPrice, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("UnitPriceAmount")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            money.Property(m => m.Currency)
                .HasColumnName("UnitPriceCurrency")
                .HasMaxLength(3)
                .IsRequired();

            money.Ignore(m => m.Symbol);
            money.Ignore(m => m.Formatted);
        });

        // Configure TotalAmount as a regular owned entity
        builder.OwnsOne(si => si.TotalAmount, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("TotalAmount")
                .HasColumnType("decimal(18,2)");

            money.Property(m => m.Currency)
                .HasColumnName("TotalAmountCurrency")
                .HasMaxLength(3);

            money.Ignore(m => m.Symbol);
            money.Ignore(m => m.Formatted);
        });
    }
}