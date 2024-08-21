namespace Order.DDD.Demo.UseCase.Exception;

public class OrderNotFoundException : System.Exception
{
    public OrderNotFoundException(string message) : base(message)
    {
    }
}