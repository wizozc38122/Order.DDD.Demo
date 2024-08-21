namespace Order.DDD.Demo.UseCase.Port.In;

/// <summary>
/// 建立訂單服務
/// </summary>
public interface ICreateOrderService
{
    Task<Guid> HandleAsync(Guid customerId, List<OrderItemInput> orderItems);
}