using FluentAssertions;
using NSubstitute;
using Order.DDD.Demo.Entity;
using Order.DDD.Demo.UseCase;
using Order.DDD.Demo.UseCase.Exception;
using Order.DDD.Demo.UseCase.Port.In;
using Order.DDD.Demo.UseCase.Port.Out;

namespace Order.DDD.Demo.UseCaseTest;

public class PickOrderItemsServiceTest
{
    private IOrderOutPort _orderOutPort;

    public PickOrderItemsServiceTest()
    {
        _orderOutPort = Substitute.For<IOrderOutPort>();
    }

    protected IPickOrderItemsService GetSystemUnderTest()
    {
        return new PickOrderItemsService(_orderOutPort);
    }

    [Fact]
    public async Task HandleAsyncTest_輸入符合規格參數_訂單存在_訂單狀態變更為揀貨中_成功儲存訂單()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var order = new Entity.Order(new OrderId(Guid.NewGuid()),
            new CustomerId(Guid.NewGuid()), DateTimeOffset.Now,
            new List<Entity.OrderItem>
            {
                new(new OrderItemId(Guid.NewGuid()), 1, 100),
                new(new OrderItemId(Guid.NewGuid()), 2, 200)
            });
        order.ConfirmPayment();
        _orderOutPort.GetAsync(orderId).Returns(order);
        _orderOutPort.UpdateAsync(order).Returns(true);

        // Act
        var sut = GetSystemUnderTest();
        await sut.HandleAsync(orderId);

        // Assert
        await _orderOutPort.Received(1).UpdateAsync(order);
        order.Status.Should().Be(Status.PickingInProgress);
    }

    [Fact]
    public async Task HandleAsyncTest_輸入符合規格參數_訂單存在_訂單狀態變更為揀貨中_儲存訂單失敗應拋出OrderChangeStatusFailedException()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var order = new Entity.Order(new OrderId(Guid.NewGuid()),
            new CustomerId(Guid.NewGuid()), DateTimeOffset.Now,
            new List<OrderItem>
            {
                new(new OrderItemId(Guid.NewGuid()), 1, 100),
                new(new OrderItemId(Guid.NewGuid()), 2, 200)
            });
        order.ConfirmPayment();
        _orderOutPort.GetAsync(orderId).Returns(order);
        _orderOutPort.UpdateAsync(order).Returns(false);

        // Act
        var sut = GetSystemUnderTest();
        var act = async () => await sut.HandleAsync(orderId);

        // Assert
        await act.Should().ThrowAsync<OrderChangeStatusFailedException>();
        order.Status.Should().Be(Status.PickingInProgress);
        await _orderOutPort.Received(1).UpdateAsync(order);
    }

    [Fact]
    public async Task HandleAsyncTest_輸入符合規格參數_訂單不存在_應拋出OrderNotFoundException()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        _orderOutPort.GetAsync(Arg.Any<Guid>()).Returns(Entity.Order.Null);

        // Act
        var sut = GetSystemUnderTest();
        var act = async () => await sut.HandleAsync(orderId);

        // Assert
        await act.Should().ThrowAsync<OrderNotFoundException>();
        await _orderOutPort.Received(1).GetAsync(orderId);
    }
}