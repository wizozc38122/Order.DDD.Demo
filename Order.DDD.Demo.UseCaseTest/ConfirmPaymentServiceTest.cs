using FluentAssertions;
using NSubstitute;
using Order.DDD.Demo.Entity;
using Order.DDD.Demo.UseCase;
using Order.DDD.Demo.UseCase.Exception;
using Order.DDD.Demo.UseCase.Port.In;
using Order.DDD.Demo.UseCase.Port.Out;

namespace Order.DDD.Demo.UseCaseTest;

public class ConfirmPaymentServiceTest
{
    private readonly IOrderOutPort _orderOutPort;

    public ConfirmPaymentServiceTest()
    {
        _orderOutPort = Substitute.For<IOrderOutPort>();
    }

    protected IConfirmPaymentService GetSystemUnderTest()
    {
        return new ConfirmPaymentService(_orderOutPort);
    }

    [Fact]
    public async Task HandleAsyncTest_輸入符合規格參數_訂單存在_應成功確認付款_更新訂單狀態成功為付款完成()
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
        _orderOutPort.GetAsync(orderId).Returns(order);
        _orderOutPort.UpdateAsync(order).Returns(true);

        // Act
        var sut = GetSystemUnderTest();
        await sut.HandleAsync(orderId);

        // Assert
        await _orderOutPort.Received(1).UpdateAsync(order);
        order.Status.Should().Be(Status.Paid);
    }

    [Fact]
    public async Task HandleAsyncTest_輸入符合規格參數_訂單存在_更新訂單狀態失敗拋出OrderChangeStatusFailedException()
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
        _orderOutPort.GetAsync(orderId).Returns(order);
        _orderOutPort.UpdateAsync(order).Returns(false);

        // Act
        var sut = GetSystemUnderTest();
        var actual = async () => await sut.HandleAsync(orderId);

        // Assert
        await actual.Should().ThrowAsync<OrderChangeStatusFailedException>();
        await _orderOutPort.Received(1).UpdateAsync(Arg.Any<Entity.Order>());
    }

    [Fact]
    public async Task HandleAsyncTest_輸入不存在的OrderId_應拋出OrderNotFoundException()
    {
        // Arrange
        var orderId = new OrderId(Guid.NewGuid());
        _orderOutPort.GetAsync(Arg.Any<Guid>()).Returns(Entity.Order.Null);

        // Act
        var sut = GetSystemUnderTest();
        var actual = async () => await sut.HandleAsync(orderId);

        // Assert
        await actual.Should().ThrowAsync<OrderNotFoundException>();
    }
}