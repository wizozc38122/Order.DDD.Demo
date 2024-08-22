namespace Order.DDD.Demo.SeedWork;

/// <summary>
/// ValueObject
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract record ValueObject<T> where T : ValueObject<T>;