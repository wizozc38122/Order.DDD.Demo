namespace Order.DDD.Demo.Entity.Exception;

/// <summary>
/// 訂單非付款狀態 
/// </summary>
public class OrderNonPendingException : System.Exception
{
    public OrderNonPendingException(string message) : base(message)
    {
    }
}