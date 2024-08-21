namespace Order.DDD.Demo.UseCase.Port.In;

/// <summary>
/// 訂單項目輸入
/// </summary>
public class OrderItemInput
{
    /// <summary>
    /// 訂單項目Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 數量
    /// </summary>
    /// <returns></returns>
    public int Quantity { get; set; }

    /// <summary>
    /// 單價
    /// </summary>
    public decimal Price { get; set; }
}