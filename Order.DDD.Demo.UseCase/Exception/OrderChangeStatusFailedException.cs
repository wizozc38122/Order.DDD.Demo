namespace Order.DDD.Demo.UseCase.Exception;

public class OrderChangeStatusFailedException : System.Exception
{
    public OrderChangeStatusFailedException(string message) : base(message)
    {
    }
}