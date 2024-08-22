namespace Order.DDD.Demo.UseCase.Exception;

/// <summary>
/// 建立訂單失敗
/// </summary>
public class CreateOrderFailedException : System.Exception
{
    public CreateOrderFailedException(string message) : base(message)
    {
    }
}