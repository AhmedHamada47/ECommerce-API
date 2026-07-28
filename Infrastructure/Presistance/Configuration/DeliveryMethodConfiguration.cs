using Domain.Models.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Presistance.Configuration
{
    public class DeliveryMethodConfiguration : IEntityTypeConfiguration<DeliveryMethod>
    {
        public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
        {
            builder.ToTable("deliveryMethods");

            builder.HasKey(dm => dm.Id);

            builder.Property(dm => dm.ShortName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(dm => dm.Description)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(dm => dm.DeliveryTime)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(dm => dm.Price)
                .HasColumnType("decimal(10,3)");
        }
    }
}
