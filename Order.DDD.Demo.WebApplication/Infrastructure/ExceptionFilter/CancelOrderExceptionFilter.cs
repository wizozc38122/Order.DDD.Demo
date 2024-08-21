using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Order.DDD.Demo.Entity.Exception;
using Order.DDD.Demo.UseCase.Exception;

namespace Order.DDD.Demo.WebApplication.Infrastructure.ExceptionFilter;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class CancelOrderExceptionFilter : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        switch (context.Exception)
        {
            case OrderChangeStatusFailedException:
                context.Result = new ObjectResult("訂單取消失敗")
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
                context.ExceptionHandled = true;
                break;

            case OrderNotFoundException e:
                context.Result = new NotFoundObjectResult(e.Message);
                context.ExceptionHandled = true;
                break;

            case OrderCancellationReasonCannotBeEmptyException e:
                context.Result = new BadRequestObjectResult(e.Message);
                context.ExceptionHandled = true;
                break;

            case OrderCannotBeCanceledException e:
                context.Result = new BadRequestObjectResult(e.Message);
                context.ExceptionHandled = true;
                break;
        }

        base.OnException(context);
    }
}