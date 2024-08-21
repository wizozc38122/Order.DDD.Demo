namespace Order.DDD.Demo.UseCase.Port.In;

/// <summary>
/// 取消訂單服務
/// </summary>
public interface ICancelOrderService
{
    Task HandleAsync(Guid orderId, string reason);
}