using Order.DDD.Demo.SeedWork;

namespace Order.DDD.Demo.Entity;

/// <summary>
/// 訂單項目來源Id
/// </summary>
public record OrderItemReferenceId : ValueObject<OrderItemReferenceId>
{
    public Guid Value { get; }

    public OrderItemReferenceId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException("ReferenceOrderItem id cannot be empty", nameof(value));
        }

        Value = value;
    }

    public static OrderItemId NewItemId()
    {
        return new OrderItemId(Guid.NewGuid());
    }

    public static OrderItemId FromGuid(Guid guid)
    {
        return new OrderItemId(guid);
    }

    public static implicit operator Guid(OrderItemReferenceId self) => self.Value;

    public static implicit operator OrderItemReferenceId(Guid value) => new(value);
}