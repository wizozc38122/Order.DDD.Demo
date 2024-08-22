using Microsoft.AspNetCore.Mvc;
using Order.DDD.Demo.UseCase.Port.In;
using Order.DDD.Demo.WebApplication.Infrastructure.ExceptionFilter;

namespace Order.DDD.Demo.WebApplication.Controller;

/// <summary>
/// 訂單 API
/// </summary>
/// <param name="createOrderService"></param>
/// <param name="cancelOrderService"></param>
[ApiController]
[Route("[controller]")]
public class OrderController(ICreateOrderService createOrderService, ICancelOrderService cancelOrderService)
    : ControllerBase
{
    /// <summary>
    /// 建立訂單
    /// </summary>
    /// <param name="customerId"></param>
    /// <param name="orderItems"></param>
    /// <returns></returns>
    [HttpPost("{customerId:guid}/Create")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [CreateOrderExceptionFilter]
    public async Task<ActionResult> CreateOrderAsync(Guid customerId, List<OrderItemInput> orderItems)
    {
        var orderId = await createOrderService.HandleAsync(customerId, orderItems);
        return Ok(orderId);
    }

    /// <summary>
    /// 取消訂單 (從支付系統)
    /// </summary>
    /// <param name="orderId"></param>
    /// <returns></returns>
    [HttpPost("{orderId:guid}/Cancel/Payment")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [CancelOrderExceptionFilter]
    public async Task<ActionResult> CancelOrderFromPaymentAsync(Guid orderId)
    {
        await cancelOrderService.HandleAsync(orderId, "支付失敗");
        return Ok();
    }
}