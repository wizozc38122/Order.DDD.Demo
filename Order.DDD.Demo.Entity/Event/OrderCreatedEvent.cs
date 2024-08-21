using Order.DDD.Demo.SeedWork;

namespace Order.DDD.Demo.Entity.Event;

/// <summary>
/// 建立訂單事件
/// </summary>
/// <param name="OrderId"></param>
/// <param name="CustomerId"></param>
public record OrderCreatedEvent(OrderId OrderId, CustomerId CustomerId, DateTimeOffset CreateTime, IList<OrderItem> OrderItems) : DomainEvent;