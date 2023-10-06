using Domain.Customers;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructure.Persistences.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");
        builder.HasKey(c => c.Id);
        builder.Property(p => p.Id)
            .HasConversion(
                customerId => customerId.Value,
                value => new CustomerId(value)
            );

        builder.Property(p => p.Name).HasMaxLength(50);
        builder.Property(p => p.LastName).HasMaxLength(50);
        builder.Property(p => p.Email).HasMaxLength(255);
        builder.HasIndex(p => p.Email).IsUnique();
        builder.Ignore(i => i.FullName);

        builder.Property(c => c.PhoneNumber)
            .HasConversion(
                phoneNumber => phoneNumber.Value,
                value => PhoneNumber.Create(value)!
            )
            .HasMaxLength(9);

        builder.OwnsOne(o => o.Address, addressBuilder =>
        {
            addressBuilder.Property(a => a.Country).HasMaxLength(3);
            addressBuilder.Property(a => a.Line1).HasMaxLength(20);
            addressBuilder.Property(a => a.Line2).HasMaxLength(20).IsRequired(false);
            addressBuilder.Property(a => a.City).HasMaxLength(40);
            addressBuilder.Property(a => a.State).HasMaxLength(40);
            addressBuilder.Property(a => a.ZipCode).HasMaxLength(10).IsRequired(false);
        });

        builder.Property(p => p.Active);
    }
}