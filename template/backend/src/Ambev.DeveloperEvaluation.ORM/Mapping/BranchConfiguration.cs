using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class BranchConfiguration : IEntityTypeConfiguration<Branch>
{
    public void Configure(EntityTypeBuilder<Branch> builder)
    {
        builder.ToTable("Branches");

        // Id is already the key from BaseEntity, no need to configure it again
        // Just configure the other properties

        builder.Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(b => b.PhoneNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(b => b.Status)
            .IsRequired()
            .HasConversion<string>();

        // Configure the Address value object
        builder.OwnsOne(b => b.Address, address =>
        {
            address.Property(a => a.Street).HasColumnName("Street").IsRequired().HasMaxLength(100);
            address.Property(a => a.City).HasColumnName("City").IsRequired().HasMaxLength(50);
            address.Property(a => a.State).HasColumnName("State").IsRequired().HasMaxLength(50);
            address.Property(a => a.PostalCode).HasColumnName("PostalCode").IsRequired().HasMaxLength(20);
            address.Property(a => a.Country).HasColumnName("Country").IsRequired().HasMaxLength(50);
        });
    }
}