namespace Order.DDD.Demo.Entity.Exception;

/// <summary>
/// 訂單非揀貨進行中狀態 
/// </summary>
public class OrderNonPickingInProgressException : System.Exception
{
    public OrderNonPickingInProgressException(string message) : base(message)
    {
    }
}