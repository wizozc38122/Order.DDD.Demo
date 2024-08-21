namespace Order.DDD.Demo.UseCase.Port.In;

/// <summary>
/// 訂單揀貨服務
/// </summary>
public interface IPickOrderItemsService
{
    Task HandleAsync(Guid orderId);
}