namespace Order.DDD.Demo.UseCase.Port.In;

/// <summary>
/// 確認付款服務
/// </summary>
public interface IConfirmPaymentService
{
    /// <summary>
    /// 處理確認付款
    /// </summary>
    /// <param name="orderId"></param>
    /// <returns></returns>
    Task HandleAsync(Guid orderId);
}