namespace Order.DDD.Demo.UseCase.Exception;

public class CreateOrderFaildException : System.Exception
{
    public CreateOrderFaildException(string message) : base(message)
    {
    }
}