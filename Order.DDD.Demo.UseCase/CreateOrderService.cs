using Order.DDD.Demo.Entity;
using Order.DDD.Demo.UseCase.Exception;
using Order.DDD.Demo.UseCase.Port.In;
using Order.DDD.Demo.UseCase.Port.Out;

namespace Order.DDD.Demo.UseCase;

public class CreateOrderService(IOrderOutPort orderOutPort, TimeProvider timeProvider) : ICreateOrderService
{
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

        throw new CreateOrderFaildException("建立訂單失敗");
    }
}