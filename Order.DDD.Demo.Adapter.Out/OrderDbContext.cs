using Microsoft.EntityFrameworkCore;
using Order.DDD.Demo.Adapter.Out.Configuration;

namespace Order.DDD.Demo.Adapter.Out;

public class OrderDbContext : DbContext
{
    public DbSet<Entity.Order> Order { get; set; }

    public OrderDbContext(DbContextOptions<OrderDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new OrderConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}