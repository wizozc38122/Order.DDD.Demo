namespace Order.DDD.Demo.Entity.Exception;

/// <summary>
/// 訂單項目數量不可小於等於0
/// </summary>
public class OrderItemQuantityLessThanOrEqualToZeroException : System.Exception
{
    public OrderItemQuantityLessThanOrEqualToZeroException(string message) : base(message)
    {
    }
}