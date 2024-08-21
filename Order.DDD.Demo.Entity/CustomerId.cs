using Order.DDD.Demo.SeedWork;

namespace Order.DDD.Demo.Entity;

/// <summary>
/// 顧客 Id
/// </summary>
public record CustomerId : ValueObject<CustomerId>
{
    public Guid Value { get; }

    public CustomerId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException("CustomerId id cannot be empty", nameof(value));
        }

        Value = value;
    }

    public static OrderId FromGuid(Guid guid)
    {
        return new OrderId(guid);
    }

    public static implicit operator Guid(CustomerId self) => self.Value;

    public static implicit operator CustomerId(Guid value) => new(value);
}