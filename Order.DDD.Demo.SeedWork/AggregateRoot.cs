using System.Collections.ObjectModel;

namespace Order.DDD.Demo.SeedWork;

/// <summary>
/// 聚合根
/// </summary>
/// <typeparam name="TId"></typeparam>
public abstract class AggregateRoot<TId> where TId : ValueObject<TId>
{
    /// <summary>
    /// 識別 Id
    /// </summary>
    public TId Id { get; protected set; }

    /// <summary>
    /// 尚未提交的事件 
    /// </summary>
    private IList<DomainEvent> UncommittedEvents { get; } = new List<DomainEvent>();

    /// <summary>
    /// 實作： 處理領域事件
    /// </summary>
    /// <param name="domainEvent"></param>
    protected abstract void When(DomainEvent domainEvent);

    /// <summary>
    /// Entity 事件處理
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="domainEvent"></param>
    protected void ApplyToEntity(IInternalEventHandler entity, DomainEvent domainEvent)
    {
        entity.Handle(domainEvent);
    }

    /// <summary>
    /// 應用領域事件 
    /// </summary>
    /// <param name="domainEvent"></param>
    protected void Apply(DomainEvent domainEvent)
    {
        When(domainEvent);
    }

    /// <summary>
    /// 新增領域事件
    /// </summary>
    /// <param name="domainEvent">The domain event</param>
    protected void RaiseEvent(DomainEvent domainEvent)
    {
        UncommittedEvents.Add(domainEvent);
    }

    /// <summary>
    /// Gets the uncommitted events
    /// </summary>
    /// <returns>A read only list of i domain event</returns>
    public IReadOnlyList<DomainEvent> GetUncommittedEvents()
    {
        return new ReadOnlyCollection<DomainEvent>(UncommittedEvents);
    }

    /// <summary>
    /// Describes whether this instance is changed
    /// </summary>
    /// <returns>The bool</returns>
    public bool IsChanged()
    {
        return UncommittedEvents.Any();
    }

    /// <summary>
    /// Clears the domain events.
    /// </summary>
    public void ClearUncommittedEvents()
    {
        UncommittedEvents.Clear();
    }
}