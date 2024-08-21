using Order.DDD.Demo.UseCase.Exception;
using Order.DDD.Demo.UseCase.Port.In;
using Order.DDD.Demo.UseCase.Port.Out;

namespace Order.DDD.Demo.UseCase;

public class PickOrderItemsService(IOrderOutPort orderOutPort) : IPickOrderItemsService
{
    public async Task HandleAsync(Guid orderId)
    {
        // 取得訂單
        var order = await orderOutPort.GetAsync(orderId);
        if (order.IsNull())
        {
            throw new OrderNotFoundException("找不到訂單");
        }

        // 訂單揀貨
        order.PickOrderItems();

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