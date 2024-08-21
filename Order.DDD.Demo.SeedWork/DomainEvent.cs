namespace Order.DDD.Demo.SeedWork;

public record DomainEvent
{
    public DateTime CreateAt { get; set; } = DateTime.Now;
    public Guid EventId { get; set; } = Guid.NewGuid();
}