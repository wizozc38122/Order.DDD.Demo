using Order.DDD.Demo.Entity;
using Order.DDD.Demo.UseCase.Exception;
using Order.DDD.Demo.UseCase.Port.In;
using Order.DDD.Demo.UseCase.Port.Out;

namespace Order.DDD.Demo.UseCase;

/// <summary>
/// 建立訂單服務
/// </summary>
/// <param name="orderOutPort"></param>
/// <param name="timeProvider"></param>
public class CreateOrderService(IOrderOutPort orderOutPort, TimeProvider timeProvider) : ICreateOrderService
{
    /// <summary>
    /// 處理建立訂單
    /// </summary>
    /// <param name="customerId"></param>
    /// <param name="orderItems"></param>
    /// <returns></returns>
    /// <exception cref="CreateOrderFailedException"></exception>
    public async Task<Guid> HandleAsync(Guid customerId, List<OrderItemInput> orderItems)
    {
        // 產生訂單Id
        var orderId = await orderOutPort.GenerateIdAsync();

        // 建立訂單
        var order = new Entity.Order(orderId, customerId, timeProvider.GetLocalNow(),
            orderItems.Select(x => new OrderItem(new OrderItemId(x.Id), x.Quantity, x.Price)).ToList());

        // 儲存訂單
        var saveResult = await orderOutPort.SaveAsync(order);

        if (saveResult)
        {
            // 發送訂單建立事件
            // ...

            return orderId;
        }

        throw new CreateOrderFailedException("建立訂單失敗");
    }
}