using Order.DDD.Demo.Entity;

namespace Order.DDD.Demo.UseCase.Port.Out;

public class OrderDataModel
{
    /// <summary>
    /// 顧客 Id
    /// </summary>
    public Guid CustomerId { get; set; }

    /// <summary>
    /// 狀態
    /// </summary>
    public Status Status { get; set; }

    /// <summary>
    /// 訂單項目
    /// </summary>
    public IList<OrderItemDataModel> OrderItems { get; set; }

    /// <summary>
    /// 總金額
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// 建立時間
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// 取消時間
    /// </summary>
    public DateTime CancelledAt { get; set; }

    /// <summary>
    /// 取消原因
    /// </summary>
    public string? CancellationReason { get; set; }
}