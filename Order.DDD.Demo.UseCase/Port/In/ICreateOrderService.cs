namespace Order.DDD.Demo.UseCase.Port.In;

/// <summary>
/// 建立訂單服務
/// </summary>
public interface ICreateOrderService
{
    /// <summary>
    /// 處理建立訂單
    /// </summary>
    /// <param name="customerId"></param>
    /// <param name="orderItems"></param>
    /// <returns></returns>
    Task<Guid> HandleAsync(Guid customerId, List<OrderItemInput> orderItems);
}