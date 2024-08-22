namespace Order.DDD.Demo.UseCase.Port.In;

/// <summary>
/// 取消訂單服務
/// </summary>
public interface ICancelOrderService
{
    /// <summary>
    /// 處理取消訂單
    /// </summary>
    /// <param name="orderId"></param>
    /// <param name="reason"></param>
    /// <returns></returns>
    Task HandleAsync(Guid orderId, string reason);
}