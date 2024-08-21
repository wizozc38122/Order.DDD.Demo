namespace Order.DDD.Demo.SeedWork;

public abstract record ValueObject<T> where T : ValueObject<T>;