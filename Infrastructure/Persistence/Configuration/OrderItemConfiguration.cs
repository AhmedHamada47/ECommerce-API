using Domain.Models.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("orderItems");

            builder.HasKey(oi => oi.Id);

            builder.Property(oi => oi.ProductName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(oi => oi.PictureUrl)
                .HasMaxLength(500);

            builder.Property(oi => oi.Price)
                .HasColumnType("decimal(10,3)");

            builder.Property(oi => oi.Quantity)
                .IsRequired();
        }
    }
}
