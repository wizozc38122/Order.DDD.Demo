namespace Order.DDD.Demo.SeedWork;

/// <summary>
/// 內部事件處理
/// </summary>
public interface IInternalEventHandler
{
    /// <summary>
    /// 處理領域事件
    /// </summary>
    /// <param name="domainEvent"></param>
    public void Handle(DomainEvent domainEvent);
}