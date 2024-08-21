namespace Order.DDD.Demo.UseCase.Port.In;

/// <summary>
/// 完成訂單服務
/// </summary>
public interface ICompleteOrderService
{
    Task HandleAsync(Guid orderId);
}