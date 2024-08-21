using FluentAssertions;
using Microsoft.Extensions.Time.Testing;
using NSubstitute;
using Order.DDD.Demo.UseCase;
using Order.DDD.Demo.UseCase.Exception;
using Order.DDD.Demo.UseCase.Port.In;
using Order.DDD.Demo.UseCase.Port.Out;

namespace Order.DDD.Demo.UseCaseTest;

public class CreateOrderServiceTest
{
    private readonly IOrderOutPort _orderOutPort;
    private readonly TimeProvider _timeProvider;

    public CreateOrderServiceTest()
    {
        _orderOutPort = Substitute.For<IOrderOutPort>();
        _timeProvider = new FakeTimeProvider();
    }

    protected ICreateOrderService GetSystemUnderTest()
    {
        return new CreateOrderService(_orderOutPort, _timeProvider);
    }

    [Fact]
    public async Task HandleAsyncTest_輸入符合規格參數_建立儲存成功_並回傳OrderId()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var orderItems = new List<OrderItemInput>
        {
            new ()
            {
                Id = Guid.NewGuid(),
                Quantity = 1,
                Price = 100
            }
        };
        var orderId = Guid.NewGuid();
        _orderOutPort.GenerateIdAsync().Returns(orderId);
        _orderOutPort.SaveAsync(Arg.Any<Entity.Order>()).Returns(true);

        // Act
        var sut = GetSystemUnderTest();
        var actual =  await sut.HandleAsync(customerId, orderItems);

        // Assert
        await _orderOutPort.Received(1).SaveAsync(Arg.Any<Entity.Order>());
        actual.Should().Be(orderId);
    }
    
    [Fact]
    public async Task HandleAsyncTest_輸入符合規格參數_建立儲存失敗_拋出CreateOrderFaildException(){
        // Arrange
        var customerId = Guid.NewGuid();
        var orderItems = new List<OrderItemInput>
        {
            new ()
            {
                Id = Guid.NewGuid(),
                Quantity = 1,
                Price = 100
            }
        };
        var orderId = Guid.NewGuid();
        _orderOutPort.GenerateIdAsync().Returns(orderId);
        _orderOutPort.SaveAsync(Arg.Any<Entity.Order>()).Returns(false);

        // Act
        var sut = GetSystemUnderTest();
        Func<Task> act = async () => await sut.HandleAsync(customerId, orderItems);
        
        // Assert
        await act.Should().ThrowAsync<CreateOrderFaildException>();
        await _orderOutPort.Received(1).SaveAsync(Arg.Any<Entity.Order>());
    }
}