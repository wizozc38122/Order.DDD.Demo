namespace Order.DDD.Demo.SeedWork;

public abstract class Entity<TId> : IInternalEventHandler where TId : ValueObject<TId>
{
    /// <summary>
    /// 外部事件處理
    /// </summary>
    private readonly Action<DomainEvent> _applier;

    /// <summary>
    /// 識別Id
    /// </summary>
    public TId Id { get; protected set; }

    /// <summary>
    /// Constructor: 注入外部事件處理
    /// </summary>
    /// <param name="applier"></param>
    protected Entity(Action<DomainEvent>? applier) => _applier = applier ?? (domainEvent => { });

    /// <summary>
    /// 實作： 處理領域事件
    /// </summary>
    /// <param name="event"></param>
    protected abstract void When(DomainEvent @event);

    /// <summary>
    /// 應用領域事件
    /// </summary>
    /// <param name="event"></param>
    protected void Apply(DomainEvent @event)
    {
        When(@event);
        _applier(@event);
    }

    /// <summary>
    /// 應用領域事件
    /// </summary>
    /// <param name="domainEvent"></param>
    public void Handle(DomainEvent domainEvent) => When(domainEvent);
}