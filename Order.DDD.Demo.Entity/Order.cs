using Order.DDD.Demo.Entity.Event;
using Order.DDD.Demo.Entity.Exception;
using Order.DDD.Demo.SeedWork;

namespace Order.DDD.Demo.Entity;

public class Order : AggregateRoot<OrderId>, INullObject
{
    /// <summary>
    /// 顧客 Id
    /// </summary>
    public CustomerId CustomerId { get; private set; }

    /// <summary>
    /// 狀態
    /// </summary>
    public Status Status { get; private set; }

    /// <summary>
    /// 訂單項目
    /// </summary>
    public List<OrderItem> OrderItems { get; private set; } = [];

    /// <summary>
    /// 總金額
    /// </summary>
    public decimal TotalAmount
    {
        get => OrderItems.Sum(x => x.TotalPrice);
        private set { }
    }

    /// <summary>
    /// 建立時間
    /// </summary>
    public DateTimeOffset CreateTime { get; private set; }

    /// <summary>
    /// 取消時間
    /// </summary>
    public DateTimeOffset? CancelledTime { get; private set; }

    /// <summary>
    /// 取消原因
    /// </summary>
    public string? CancellationReason { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="orderId"></param>
    /// <param name="customerId"></param>
    /// <param name="createAt"></param>
    /// <param name="orderItems"></param>
    /// <exception cref="OrderItemEmptyException"></exception>
    public Order(OrderId orderId, CustomerId customerId, DateTimeOffset createAt,
        IList<OrderItem> orderItems)
    {
        if (orderItems.Any() == false)
        {
            throw new OrderItemEmptyException("訂單項目不可為空");
        }

        if (orderItems.Any(x => x.Quantity <= 0))
        {
            throw new OrderItemQuantityLessThanOrEqualToZeroException(
                $"訂單項目數量不能小於等於0, 錯誤的OrderItemId：{string.Join(",", orderItems
                    .Where(x => x.Quantity <= 0)
                    .Select(x => x.OrderItemId))}");
        }

        if (orderItems.Any(x => x.Price < 0))
        {
            throw new OrderItemPriceLessThanZeroException(
                $"訂單項目價格不能小於0, 錯誤的OrderItemId：{string.Join(",", orderItems
                    .Where(x => x.Price < 0)
                    .Select(x => x.OrderItemId))}");
        }

        Apply(new OrderCreatedEvent(orderId, customerId, createAt, orderItems));
    }

    /// <summary>
    /// 處理領域事件
    /// </summary>
    /// <param name="domainEvent"></param>
    protected sealed override void When(DomainEvent domainEvent)
    {
        switch (domainEvent)
        {
            case OrderCreatedEvent e:
                Id = e.OrderId;
                CustomerId = e.CustomerId;
                Status = Status.PendingPayment;
                CreateTime = e.CreateTime;
                OrderItems = [];
                foreach (var orderItem in e.OrderItems)
                {
                    OrderItems.Add(orderItem);
                }

                break;

            case OrderPaidEvent e:
                Status = Status.Paid;
                break;

            case OrderPickingStartedEvent e:
                Status = Status.PickingInProgress;
                break;

            case OrderCompletedEvent e:
                Status = Status.Completed;
                break;

            case OrderCancelledEvent e:
                Status = Status.Cancelled;
                CancelledTime = e.CancelledTime;
                CancellationReason = e.Reason;
                break;
        }
    }

    /// <summary>
    /// 確認付款
    /// </summary>
    /// <exception cref="OrderItemEmptyException"></exception>
    public void ConfirmPayment()
    {
        if (Status != Status.PendingPayment)
        {
            throw new OrderNonPendingException("訂單狀態非待付款");
        }

        Apply(new OrderPaidEvent(Id));
    }

    /// <summary>
    /// 揀貨
    /// </summary>
    /// <exception cref="OrderNotPaidException"></exception>
    public void PickOrderItems()
    {
        if (Status != Status.Paid)
        {
            throw new OrderNotPaidException("訂單狀態非已付款");
        }

        Apply(new OrderPickingStartedEvent(Id));
    }

    /// <summary>
    /// 完成訂單
    /// </summary>
    /// <exception cref="OrderNonPickingInProgressException"></exception>
    public void CompleteOrder()
    {
        if (Status != Status.PickingInProgress)
        {
            throw new OrderNonPickingInProgressException("訂單狀態非揀貨進行中");
        }

        Apply(new OrderCompletedEvent(Id));
    }

    /// <summary>
    /// 取消訂單
    /// </summary>
    /// <param name="reason"></param>
    /// <param name="cancelledTime"></param>
    /// <exception cref="OrderCannotBeCanceledException"></exception>
    public void CancelOrder(string reason, DateTimeOffset cancelledTime)
    {
        if (Status is Status.Completed or Status.Cancelled)
        {
            throw new OrderCannotBeCanceledException("訂單不可取消狀態");
        }

        if (string.IsNullOrWhiteSpace(reason))
        {
            throw new OrderCancellationReasonCannotBeEmptyException("取消原因不可為空");
        }

        Apply(new OrderCancelledEvent(Id, reason, cancelledTime));
    }

    protected Order()
    {
    }

    private static readonly NullOrder? _null;

    public static Order Null = _null ??= new NullOrder();

    public virtual bool IsNull()
    {
        return false;
    }

    private class NullOrder : Order
    {
        public override bool IsNull()
        {
            return true;
        }
    }
}

public enum Status
{
    /// <summary>
    /// 待付款
    /// </summary>
    PendingPayment,

    /// <summary>
    /// 已付款
    /// </summary>
    Paid,

    /// <summary>
    /// 揀貨進行中
    /// </summary>
    PickingInProgress,

    /// <summary>
    /// 完成
    /// </summary>
    Completed,

    /// <summary>
    /// 取消
    /// </summary>
    Cancelled,
}