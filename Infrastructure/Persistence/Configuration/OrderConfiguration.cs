using Domain.Models.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("orders");

            builder.HasKey(o => o.Id);

            builder.Property(o => o.BuyerEmail)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(o => o.OrderDate)
                .IsRequired();

            builder.OwnsOne(o => o.ShippingAddress, address =>
            {
                address.Property(a => a.FirstName).IsRequired().HasMaxLength(100);
                address.Property(a => a.LastName).IsRequired().HasMaxLength(100);
                address.Property(a => a.Street).IsRequired().HasMaxLength(200);
                address.Property(a => a.City).IsRequired().HasMaxLength(100);
                address.Property(a => a.State).HasMaxLength(100);
                address.Property(a => a.ZipCode).IsRequired().HasMaxLength(20);
            });

            builder.Property(o => o.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(o => o.Subtotal)
                .HasColumnType("decimal(10,3)");

            builder.Property(o => o.PaymentIntentId)
                .HasMaxLength(255);

            builder.HasOne(o => o.DeliveryMethod)
                .WithMany()
                .HasForeignKey(o => o.DeliveryMethodId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(o => o.OrderItems)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
