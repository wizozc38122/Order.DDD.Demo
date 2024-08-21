using Microsoft.EntityFrameworkCore;
using Order.DDD.Demo.Entity;
using Order.DDD.Demo.UseCase.Port.Out;

namespace Order.DDD.Demo.Adapter.Out.Implementation;

public class OrderRepository(OrderDbContext orderDbContext) : IOrderOutPort
{
    public async Task<Entity.Order> GetAsync(Guid orderId)
    {
        var order = await orderDbContext.Order.FindAsync(OrderId.FromGuid(orderId));
        return order ?? Entity.Order.Null;
    }

    public async Task<Guid> GenerateIdAsync()
    {
        var newOrderId = Guid.NewGuid();
        while (await CheckExist(newOrderId) is not null)
        {
            newOrderId = Guid.NewGuid();
        }

        return newOrderId;

        async Task<Entity.Order?> CheckExist(Guid orderId) =>
            await orderDbContext.Order.FindAsync(OrderId.FromGuid(orderId));
    }

    public async Task<bool> SaveAsync(Entity.Order order)
    {
        orderDbContext.Order.Add(order);
        var successCount = await orderDbContext.SaveChangesAsync();
        return successCount > 0;
    }

    public async Task<bool> UpdateAsync(Entity.Order order)
    {
        orderDbContext.Order.Update(order);
        var successCount = await orderDbContext.SaveChangesAsync();
        return successCount > 0;
    }
}