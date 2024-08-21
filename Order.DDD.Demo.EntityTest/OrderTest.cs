using FluentAssertions;
using Order.DDD.Demo.Entity;
using Order.DDD.Demo.Entity.Exception;

namespace Order.DDD.Demo.EntityTest;

public class OrderTest
{
    [Fact]
    public void CreateOrder_輸正符合規格參數_應成功建立Order()
    {
        // Arrange
        var orderId = new OrderId(Guid.NewGuid());
        var customerId = new CustomerId(Guid.NewGuid());
        var localDateTimeNow = new DateTimeOffset(new DateTime(2024, 8, 20));
        var orderItems = new List<OrderItem>
        {
            new(new OrderItemId(Guid.NewGuid()), 1, 100),
            new(new OrderItemId(Guid.NewGuid()), 2, 200)
        };

        // Act
        var act = new Entity.Order(orderId, customerId, localDateTimeNow, orderItems);

        // Assert
        act.Should().NotBeNull();
        act.Id.Should().Be(orderId);
        act.Status.Should().Be(Status.PendingPayment);
        act.OrderItems.Should().BeEquivalentTo(orderItems);
        act.TotalAmount.Should().Be(500);
        act.CreateTime.Should().Be(localDateTimeNow);
        act.CancelledTime.Should().BeNull();
        act.CancellationReason.Should().BeNull();
    }

    [Fact]
    public void CreateOrder_輸入空的OrderItems_應拋出OrderItemEmptyException()
    {
        // Arrange
        var orderId = new OrderId(Guid.NewGuid());
        var customerId = new CustomerId(Guid.NewGuid());
        var localDateTimeNow = new DateTimeOffset(new DateTime(2024, 8, 20));
        var orderItems = new List<OrderItem>();

        // Act
        Action act = () => new Entity.Order(orderId, customerId, localDateTimeNow, orderItems);

        // Assert
        act.Should().Throw<OrderItemEmptyException>();
    }

    [Fact]
    public void CreateOrder_輸入OrderItems中有數量小於等於0的OrderItem_應拋出OrderItemQuantityLessThanZeroException()
    {
        // Arrange
        var orderId = new OrderId(Guid.NewGuid());
        var customerId = new CustomerId(Guid.NewGuid());
        var localDateTimeNow = new DateTimeOffset(new DateTime(2024, 8, 20));
        var orderItems = new List<OrderItem>
        {
            new(new OrderItemId(Guid.NewGuid()), 1, 100),
            new(new OrderItemId(Guid.NewGuid()), 0, 200)
        };

        // Act
        Action act = () => new Entity.Order(orderId, customerId, localDateTimeNow, orderItems);

        // Assert
        act.Should().Throw<OrderItemQuantityLessThanOrEqualToZeroException>();
    }

    [Fact]
    public void CreateOrder_輸入OrderItems中有價格小於0的OrderItem_應拋出OrderItemPriceLessThanZeroException()
    {
        // Arrange
        var orderId = new OrderId(Guid.NewGuid());
        var customerId = new CustomerId(Guid.NewGuid());
        var localDateTimeNow = new DateTimeOffset(new DateTime(2024, 8, 20));
        var orderItems = new List<OrderItem>
        {
            new(new OrderItemId(Guid.NewGuid()), 1, 100),
            new(new OrderItemId(Guid.NewGuid()), 2, -1)
        };

        // Act
        Action act = () => new Entity.Order(orderId, customerId, localDateTimeNow, orderItems);

        // Assert
        act.Should().Throw<OrderItemPriceLessThanZeroException>();
    }

    [Fact]
    public void ConfirmPayment_訂單狀態為PendingPayment_應成功更新狀態為Paid()
    {
        // Arrange
        var orderId = new OrderId(Guid.NewGuid());
        var customerId = new CustomerId(Guid.NewGuid());
        var localDateTimeNow = new DateTimeOffset(new DateTime(2024, 8, 20));
        var orderItems = new List<OrderItem>
        {
            new(new OrderItemId(Guid.NewGuid()), 1, 100),
            new(new OrderItemId(Guid.NewGuid()), 2, 200)
        };
        var order = new Entity.Order(orderId, customerId, localDateTimeNow, orderItems);

        // Act
        order.ConfirmPayment();

        // Assert
        order.Status.Should().Be(Status.Paid);
    }

    [Fact]
    public void ConfirmPayment_訂單狀態非PendingPayment_應拋出OrderNonPendingException()
    {
        // Arrange
        var orderId = new OrderId(Guid.NewGuid());
        var customerId = new CustomerId(Guid.NewGuid());
        var localDateTimeNow = new DateTimeOffset(new DateTime(2024, 8, 20));
        var orderItems = new List<OrderItem>
        {
            new(new OrderItemId(Guid.NewGuid()), 1, 100),
            new(new OrderItemId(Guid.NewGuid()), 2, 200)
        };
        var order = new Entity.Order(orderId, customerId, localDateTimeNow, orderItems);
        order.ConfirmPayment();

        // Act
        var act = () => order.ConfirmPayment();

        // Assert
        act.Should().Throw<OrderNonPendingException>();
    }

    [Fact]
    public void PickOrderItems_訂單狀態為Paid_應成功更新狀態為PickingInProgress()
    {
        // Arrange
        var orderId = new OrderId(Guid.NewGuid());
        var customerId = new CustomerId(Guid.NewGuid());
        var localDateTimeNow = new DateTimeOffset(new DateTime(2024, 8, 20));
        var orderItems = new List<OrderItem>
        {
            new(new OrderItemId(Guid.NewGuid()), 1, 100),
            new(new OrderItemId(Guid.NewGuid()), 2, 200)
        };
        var order = new Entity.Order(orderId, customerId, localDateTimeNow, orderItems);
        order.ConfirmPayment();

        // Act
        order.PickOrderItems();

        // Assert
        order.Status.Should().Be(Status.PickingInProgress);
    }

    [Fact]
    public void PickOrderItems_訂單狀態非Paid_應拋出OrderNonPaidException()
    {
        // Arrange
        var orderId = new OrderId(Guid.NewGuid());
        var customerId = new CustomerId(Guid.NewGuid());
        var localDateTimeNow = new DateTimeOffset(new DateTime(2024, 8, 20));
        var orderItems = new List<OrderItem>
        {
            new(new OrderItemId(Guid.NewGuid()), 1, 100),
            new(new OrderItemId(Guid.NewGuid()), 2, 200)
        };
        var order = new Entity.Order(orderId, customerId, localDateTimeNow, orderItems);

        // Act
        var act = () => order.PickOrderItems();

        // Assert
        act.Should().Throw<OrderNotPaidException>();
    }

    [Fact]
    public void CompleteOrder_訂單狀態為PickingInProgress_應成功更新狀態為Completed()
    {
        // Arrange
        var orderId = new OrderId(Guid.NewGuid());
        var customerId = new CustomerId(Guid.NewGuid());
        var localDateTimeNow = new DateTimeOffset(new DateTime(2024, 8, 20));
        var orderItems = new List<OrderItem>
        {
            new(new OrderItemId(Guid.NewGuid()), 1, 100),
            new(new OrderItemId(Guid.NewGuid()), 2, 200)
        };
        var order = new Entity.Order(orderId, customerId, localDateTimeNow, orderItems);
        order.ConfirmPayment();
        order.PickOrderItems();

        // Act
        order.CompleteOrder();

        // Assert
        order.Status.Should().Be(Status.Completed);
    }

    [Fact]
    public void CompleteOrder_訂單狀態非PickingInProgress_應拋出OrderNotPickingInProgressException()
    {
        // Arrange
        var orderId = new OrderId(Guid.NewGuid());
        var customerId = new CustomerId(Guid.NewGuid());
        var localDateTimeNow = new DateTimeOffset(new DateTime(2024, 8, 20));
        var orderItems = new List<OrderItem>
        {
            new(new OrderItemId(Guid.NewGuid()), 1, 100),
            new(new OrderItemId(Guid.NewGuid()), 2, 200)
        };
        var order = new Entity.Order(orderId, customerId, localDateTimeNow, orderItems);
        order.ConfirmPayment();

        // Act
        var act = () => order.CompleteOrder();

        // Assert
        act.Should().Throw<OrderNonPickingInProgressException>();
    }

    [Fact]
    public void CancelOrder_訂單狀態非Completed及Cancelled_應成功更新狀態為Cancelled()
    {
        // Arrange
        var orderId = new OrderId(Guid.NewGuid());
        var customerId = new CustomerId(Guid.NewGuid());
        var localDateTimeNow = new DateTimeOffset(new DateTime(2024, 8, 20));
        var orderItems = new List<OrderItem>
        {
            new(new OrderItemId(Guid.NewGuid()), 1, 100),
            new(new OrderItemId(Guid.NewGuid()), 2, 200)
        };
        var order = new Entity.Order(orderId, customerId, localDateTimeNow, orderItems);
        order.ConfirmPayment();
        order.PickOrderItems();

        // Act
        order.CancelOrder("取消原因", localDateTimeNow);

        // Assert
        order.Status.Should().Be(Status.Cancelled);
        order.CancellationReason.Should().Be("取消原因");
        order.CancelledTime.Should().Be(localDateTimeNow);
    }

    [Fact]
    public void CancelOrder_訂單狀態為Completed及Cancelled_應拋出OrderCannotBeCanceledException()
    {
        // Arrange
        var orderId = new OrderId(Guid.NewGuid());
        var customerId = new CustomerId(Guid.NewGuid());
        var localDateTimeNow = new DateTimeOffset(new DateTime(2024, 8, 20));
        var orderItems = new List<OrderItem>
        {
            new(new OrderItemId(Guid.NewGuid()), 1, 100),
            new(new OrderItemId(Guid.NewGuid()), 2, 200)
        };
        var order = new Entity.Order(orderId, customerId, localDateTimeNow, orderItems);
        order.ConfirmPayment();
        order.PickOrderItems();
        order.CompleteOrder();

        // Act
        var act = () => order.CancelOrder("取消原因", localDateTimeNow);

        // Assert
        act.Should().Throw<OrderCannotBeCanceledException>();
    }

    [Fact]
    public void CancelOrder_取消原因為空_應拋出OrderCancellationReasonCannotBeEmptyException()
    {
        // Arrange
        var orderId = new OrderId(Guid.NewGuid());
        var customerId = new CustomerId(Guid.NewGuid());
        var localDateTimeNow = new DateTimeOffset(new DateTime(2024, 8, 20));
        var orderItems = new List<OrderItem>
        {
            new(new OrderItemId(Guid.NewGuid()), 1, 100),
            new(new OrderItemId(Guid.NewGuid()), 2, 200)
        };
        var order = new Entity.Order(orderId, customerId, localDateTimeNow, orderItems);
        order.ConfirmPayment();
        order.PickOrderItems();

        // Act
        var act = () => order.CancelOrder("", localDateTimeNow);

        // Assert
        act.Should().Throw<OrderCancellationReasonCannotBeEmptyException>();
    }
}