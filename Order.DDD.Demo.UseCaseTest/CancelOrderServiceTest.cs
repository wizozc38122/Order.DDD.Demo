using FluentAssertions;
using Microsoft.Extensions.Time.Testing;
using NSubstitute;
using Order.DDD.Demo.Entity;
using Order.DDD.Demo.UseCase;
using Order.DDD.Demo.UseCase.Exception;
using Order.DDD.Demo.UseCase.Port.In;
using Order.DDD.Demo.UseCase.Port.Out;

namespace Order.DDD.Demo.UseCaseTest;

public class CancelOrderServiceTest
{
    private readonly IOrderOutPort _orderOutPort;
    private readonly TimeProvider _timeProvider;
    
    public CancelOrderServiceTest()
    {
        _orderOutPort = Substitute.For<IOrderOutPort>();
        _timeProvider = new FakeTimeProvider();
    }
    
    protected ICancelOrderService GetSystemUnderTest()
    {
        return new CancelOrderService(_orderOutPort, _timeProvider);
    }
    
    [Fact]
    public async Task HandleAsyncTest_輸入符合規格參數_訂單存在_應成功取消訂單_更新訂單狀態成功為已取消()
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
        await sut.HandleAsync(orderId, "取消原因");
        
        // Assert
        await _orderOutPort.Received(1).UpdateAsync(order);
        order.Status.Should().Be(Status.Cancelled);
    }
    
    [Fact]
    public async Task HandleAsyncTest_輸入符合規格參數_訂單存在_更新訂單狀態失敗_拋出OrderChangeStatusFailedException()
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
        var act = async () => await sut.HandleAsync(orderId, "取消原因");
        
        // Assert
        await act.Should().ThrowAsync<OrderChangeStatusFailedException>();
    }
    
    [Fact]
    public async Task HandleAsyncTest_輸入符合規格參數_訂單不存在_拋出OrderNotFoundException()
    {
        // Arrange
        var orderId = new OrderId(Guid.NewGuid());
        _orderOutPort.GetAsync(orderId).Returns(Entity.Order.Null);
        
        // Act
        var sut = GetSystemUnderTest();
        var act = async () => await sut.HandleAsync(orderId, "取消原因");
        
        // Assert
        await act.Should().ThrowAsync<OrderNotFoundException>();
    }
}