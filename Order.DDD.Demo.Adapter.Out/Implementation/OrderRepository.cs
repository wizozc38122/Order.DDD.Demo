using Order.DDD.Demo.Entity;
using Order.DDD.Demo.UseCase.Port.Out;

namespace Order.DDD.Demo.Adapter.Out.Implementation;

/// <summary>
/// 訂單資料庫存取
/// </summary>
/// <param name="orderDbContext"></param>
public class OrderRepository(OrderDbContext orderDbContext) : IOrderOutPort
{
    /// <summary>
    /// 取得訂單資訊
    /// </summary>
    /// <param name="orderId"></param>
    /// <returns></returns>
    public async Task<Entity.Order> GetAsync(Guid orderId)
    {
        var order = await orderDbContext.Order.FindAsync(OrderId.FromGuid(orderId));
        return order ?? Entity.Order.Null;
    }

    /// <summary>
    /// 產生新Id
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// 儲存
    /// </summary>
    /// <param name="order"></param>
    /// <returns></returns>
    public async Task<bool> SaveAsync(Entity.Order order)
    {
        orderDbContext.Order.Add(order);
        var successCount = await orderDbContext.SaveChangesAsync();
        return successCount > 0;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="order"></param>
    /// <returns></returns>
    public async Task<bool> UpdateAsync(Entity.Order order)
    {
        orderDbContext.Order.Update(order);
        var successCount = await orderDbContext.SaveChangesAsync();
        return successCount > 0;
    }
}