namespace Order.DDD.Demo.Entity.Exception;

/// <summary>
/// 訂單項目為空
/// </summary>
public class OrderItemEmptyException : System.Exception
{
    public OrderItemEmptyException(string message) : base(message)
    {
    }
}