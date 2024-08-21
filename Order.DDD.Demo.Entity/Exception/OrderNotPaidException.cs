namespace Order.DDD.Demo.Entity.Exception;

/// <summary>
/// 訂單非付款狀態 
/// </summary>
public class OrderNotPaidException : System.Exception
{
    public OrderNotPaidException(string message) : base(message)
    {
    }
}