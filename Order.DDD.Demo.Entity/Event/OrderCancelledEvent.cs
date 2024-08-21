using Order.DDD.Demo.SeedWork;

namespace Order.DDD.Demo.Entity.Event;

/// <summary>
/// 訂單取消事件
/// </summary>
/// <param name="OrderId"></param>
/// <param name="Reason"></param>
/// <param name="CancelledTime"></param>
public record OrderCancelledEvent(Guid OrderId, string Reason, DateTimeOffset CancelledTime) : DomainEvent;