using Order.DDD.Demo.SeedWork;

namespace Order.DDD.Demo.Entity;

/// <summary>
/// 訂單項目Id
/// </summary>
public record OrderItemId : ValueObject<OrderItemId>
{
    public Guid Value { get; }

    public OrderItemId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException("ItemId id cannot be empty", nameof(value));
        }

        Value = value;
    }

    public static OrderItemId FromGuid(Guid guid)
    {
        return new OrderItemId(guid);
    }

    public static implicit operator Guid(OrderItemId self) => self.Value;

    public static implicit operator OrderItemId(Guid value) => new(value);
}