using Order.DDD.Demo.SeedWork;

namespace Order.DDD.Demo.Entity;

/// <summary>
/// 訂單Id
/// </summary>
public record OrderId : ValueObject<OrderId>
{
    public Guid Value { get; }

    public OrderId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException("Order id cannot be empty", nameof(value));
        }

        Value = value;
    }

    public static OrderId NewOrderId()
    {
        return new OrderId(Guid.NewGuid());
    }

    public static OrderId FromGuid(Guid guid)
    {
        return new OrderId(guid);
    }

    public static implicit operator Guid(OrderId self) => self.Value;

    public static implicit operator OrderId(Guid value) => new(value);
}