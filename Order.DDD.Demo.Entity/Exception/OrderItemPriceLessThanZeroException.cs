namespace Order.DDD.Demo.Entity.Exception;

/// <summary>
/// 訂單項目金額不可小於0
/// </summary>
public class OrderItemPriceLessThanZeroException : System.Exception
{
    public OrderItemPriceLessThanZeroException(string message) : base(message)
    {
    }
}