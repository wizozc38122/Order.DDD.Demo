namespace Order.DDD.Demo.UseCase.Exception;

/// <summary>
/// 訂單狀態變更失敗
/// </summary>
public class OrderChangeStatusFailedException : System.Exception
{
    public OrderChangeStatusFailedException(string message) : base(message)
    {
    }
}