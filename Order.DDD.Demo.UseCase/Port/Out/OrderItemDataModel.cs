namespace Order.DDD.Demo.UseCase.Port.Out;

public class OrderItemDataModel
{
    /// <summary>
    /// 數量
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// 價格
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// 總價
    /// </summary>
    public decimal TotalPrice { get; set; }
}