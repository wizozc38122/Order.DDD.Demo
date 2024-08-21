namespace Order.DDD.Demo.Entity.Exception;

/// <summary>
/// 訂單取消原因不可為空
/// </summary>
public class OrderCancellationReasonCannotBeEmptyException : System.Exception
{
    public OrderCancellationReasonCannotBeEmptyException(string message) : base(message)
    {
    }
}