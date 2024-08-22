namespace Order.DDD.Demo.UseCase.Port.In;

/// <summary>
/// 完成訂單服務
/// </summary>
public interface ICompleteOrderService
{
    /// <summary>
    /// 處理完成訂單
    /// </summary>
    /// <param name="orderId"></param>
    /// <returns></returns>
    Task HandleAsync(Guid orderId);
}