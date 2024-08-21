using Order.DDD.Demo.SeedWork;

namespace Order.DDD.Demo.Entity.Event;

/// <summary>
/// 訂單開始揀貨事件
/// </summary>
/// <param name="OrderId"></param>
public record OrderPickingStartedEvent(Guid OrderId) : DomainEvent;