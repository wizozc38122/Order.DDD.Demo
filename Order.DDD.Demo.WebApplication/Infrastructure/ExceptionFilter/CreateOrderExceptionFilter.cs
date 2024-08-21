using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Order.DDD.Demo.Entity.Exception;
using Order.DDD.Demo.UseCase.Exception;

namespace Order.DDD.Demo.WebApplication.Infrastructure.ExceptionFilter;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class CreateOrderExceptionFilter : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        switch (context.Exception)
        {
            case CreateOrderFaildException e:
                context.Result = new ObjectResult(e.Message)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
                context.ExceptionHandled = true;
                break;

            case OrderItemEmptyException e:
                context.Result = new BadRequestObjectResult(e.Message);
                context.ExceptionHandled = true;
                break;
            
            case OrderItemPriceLessThanZeroException e:
                context.Result = new BadRequestObjectResult(e.Message);
                context.ExceptionHandled = true;
                break;
            
            case OrderItemQuantityLessThanOrEqualToZeroException e:
                context.Result = new BadRequestObjectResult(e.Message);
                context.ExceptionHandled = true;
                break;
        }

        base.OnException(context);
    }
}