namespace Order.DDD.Demo.UseCase.Port.In;

/// <summary>
/// 確認付款服務
/// </summary>
public interface IConfirmPaymentService
{
    Task HandleAsync(Guid orderId);
}