namespace Order.DDD.Demo.SeedWork;

public interface IInternalEventHandler
{
    public void Handle(DomainEvent domainEvent);
}