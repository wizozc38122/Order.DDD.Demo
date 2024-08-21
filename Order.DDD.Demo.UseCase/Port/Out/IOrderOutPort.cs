namespace Order.DDD.Demo.UseCase.Port.Out;

public interface IOrderOutPort
{
    /// <summary>
    /// 取得訂單資訊
    /// </summary>
    /// <param name="orderId"></param>
    /// <returns></returns>
    Task<Entity.Order> GetAsync(Guid orderId);

    /// <summary>
    /// 產生新Id
    /// </summary>
    /// <returns></returns>
    Task<Guid> GenerateIdAsync();

    /// <summary>
    /// 儲存
    /// </summary>
    /// <param name="order"></param>
    /// <returns></returns>
    Task<bool> SaveAsync(Entity.Order order);

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="order"></param>
    /// <returns></returns>
    Task<bool> UpdateAsync(Entity.Order order);
}