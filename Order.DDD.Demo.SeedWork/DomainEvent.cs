namespace Order.DDD.Demo.SeedWork;

/// <summary>
/// 領域事件
/// </summary>
public record DomainEvent
{
    /// <summary>
    /// 建立時間
    /// </summary>
    public DateTime CreateAt { get; set; } = DateTime.Now;
    /// <summary>
    /// 事件 Id
    /// </summary>
    public Guid EventId { get; set; } = Guid.NewGuid();
}