using Order.DDD.Demo.SeedWork;

namespace Order.DDD.Demo.Entity;

/// <summary>
/// 訂單項目
/// </summary>
public record OrderItem(OrderItemId OrderItemId, int Quantity, decimal Price)
    : ValueObject<OrderItemId>
{
    /// <summary>
    /// 總價
    /// </summary>
    public decimal TotalPrice
    {
        get => Price * Quantity;
        private set { }
    }
}