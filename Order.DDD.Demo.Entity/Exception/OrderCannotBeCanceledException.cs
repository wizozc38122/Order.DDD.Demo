namespace Order.DDD.Demo.Entity.Exception;

/// <summary>
/// 訂單不可取消
/// </summary>
public class OrderCannotBeCanceledException : System.Exception
{
    public OrderCannotBeCanceledException(string message) : base(message)
    {
    }
}