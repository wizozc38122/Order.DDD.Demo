using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Order.DDD.Demo.Adapter.Out.Configuration;

public class OrderConfiguration : IEntityTypeConfiguration<Entity.Order>
{
    public void Configure(EntityTypeBuilder<Entity.Order> builder)
    {
        #region Order

        builder.ToTable("Order");

        builder.Property(x => x.Id)
            .HasColumnName("Id")
            .HasConversion(x => x.Value, x => x)
            .IsRequired();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.CustomerId)
            .HasColumnName("CustomerId")
            .HasConversion(x => x.Value, x => x)
            .IsRequired();

        builder.Property(x => x.Status)
            .HasColumnName("Status")
            .IsRequired();

        builder.Property(x => x.TotalAmount)
            .HasColumnName("TotalAmount")
            .IsRequired();

        builder.Property(x => x.CreateTime)
            .HasColumnName("CreatedAt")
            .IsRequired();

        builder.Property(x => x.CancelledTime)
            .HasColumnName("CancelledAt")
            .IsRequired(false);

        builder.Property(x => x.CancellationReason)
            .HasColumnName("CancellationReason")
            .IsRequired(false);

        #endregion

        #region OrderItem

        builder.OwnsMany(x => x.OrderItems, a =>
        {
            a.ToTable("OrderItem");
            a.WithOwner().HasForeignKey("OrderId");

            a.Property(x => x.OrderItemId)
                .HasColumnName("OrderItemId")
                .HasConversion(x => x.Value, x => x)
                .IsRequired();

            a.Property(x => x.Quantity)
                .HasColumnName("Quantity")
                .IsRequired();

            a.Property(x => x.Price)
                .HasColumnName("Price")
                .IsRequired();
        });

        #endregion
    }
}