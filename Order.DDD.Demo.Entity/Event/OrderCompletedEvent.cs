using Order.DDD.Demo.SeedWork;

namespace Order.DDD.Demo.Entity.Event;

/// <summary>
/// 訂單完成事件
/// </summary>
/// <param name="OrderId"></param>
public record OrderCompletedEvent(Guid OrderId) : DomainEvent;