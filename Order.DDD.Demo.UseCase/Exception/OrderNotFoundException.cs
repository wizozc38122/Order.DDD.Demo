namespace Order.DDD.Demo.UseCase.Exception;

/// <summary>
/// 訂單找不到
/// </summary>
public class OrderNotFoundException : System.Exception
{
    public OrderNotFoundException(string message) : base(message)
    {
    }
}