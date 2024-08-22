using Order.DDD.Demo.UseCase.Exception;
using Order.DDD.Demo.UseCase.Port.In;
using Order.DDD.Demo.UseCase.Port.Out;

namespace Order.DDD.Demo.UseCase;

/// <summary>
/// 取消訂單服務
/// </summary>
/// <param name="orderOutPort"></param>
/// <param name="timeProvider"></param>
public class CancelOrderService(IOrderOutPort orderOutPort, TimeProvider timeProvider) : ICancelOrderService
{
    /// <summary>
    /// 處理取消訂單
    /// </summary>
    /// <param name="orderId"></param>
    /// <param name="reason"></param>
    /// <exception cref="OrderNotFoundException"></exception>
    /// <exception cref="OrderChangeStatusFailedException"></exception>
    public async Task HandleAsync(Guid orderId, string reason)
    {
        // 取得訂單資訊
        var order = await orderOutPort.GetAsync(orderId);
        if (order.IsNull())
        {
            throw new OrderNotFoundException("找不到訂單");
        }

        // 取消訂單
        order.CancelOrder(reason, timeProvider.GetLocalNow());

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