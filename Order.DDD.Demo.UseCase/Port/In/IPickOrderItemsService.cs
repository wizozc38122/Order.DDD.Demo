namespace Order.DDD.Demo.UseCase.Port.In;

/// <summary>
/// 訂單揀貨服務
/// </summary>
public interface IPickOrderItemsService
{
    /// <summary>
    /// 處理揀貨
    /// </summary>
    /// <param name="orderId"></param>
    /// <returns></returns>
    Task HandleAsync(Guid orderId);
}