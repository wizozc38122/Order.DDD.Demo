using Order.DDD.Demo.UseCase.Exception;
using Order.DDD.Demo.UseCase.Port.In;
using Order.DDD.Demo.UseCase.Port.Out;

namespace Order.DDD.Demo.UseCase;

/// <summary>
/// 確認付款服務
/// </summary>
/// <param name="orderOutPort"></param>
public class ConfirmPaymentService(IOrderOutPort orderOutPort) : IConfirmPaymentService
{
    /// <summary>
    /// 處理確認付款
    /// </summary>
    /// <param name="orderId"></param>
    /// <exception cref="OrderNotFoundException"></exception>
    /// <exception cref="OrderChangeStatusFailedException"></exception>
    public async Task HandleAsync(Guid orderId)
    {
        // 取得訂單資訊
        var order = await orderOutPort.GetAsync(orderId);
        if (order.IsNull())
        {
            throw new OrderNotFoundException("找不到訂單");
        }

        // 確認付款
        order.ConfirmPayment();

        // 儲存訂單
        var saveResult = await orderOutPort.UpdateAsync(order);

        // 事件發送
        if (saveResult)
        {
            // 發送訂單付款確認事件
            // ...

            return;
        }

        throw new OrderChangeStatusFailedException("訂單狀態更新失敗");
    }
}