using Order.DDD.Demo.SeedWork;

namespace Order.DDD.Demo.Entity.Event;

/// <summary>
/// 訂單付款完成事件
/// </summary>
/// <param name="OrderId"></param>
public record OrderPaidEvent(OrderId OrderId) : DomainEvent;