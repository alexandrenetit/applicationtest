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

        // Owned Money Type for UnitPrice
        builder.OwnsOne(si => si.UnitPrice, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("UnitPrice")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            money.Property(m => m.Currency)
                .HasColumnName("UnitPriceCurrency")
                .HasMaxLength(3)
                .IsRequired();
        });

        // Owned Money Type for TotalAmount
        builder.OwnsOne(si => si.TotalAmount, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("TotalAmount")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            money.Property(m => m.Currency)
                .HasColumnName("TotalAmountCurrency")
                .HasMaxLength(3)
                .IsRequired();
        });

        // Relationships
        builder.HasOne(si => si.Product)
            .WithMany()
            .HasForeignKey("Id")
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        // Ignore computed properties
        builder.Ignore(si => si.TotalAmount);
    }
}