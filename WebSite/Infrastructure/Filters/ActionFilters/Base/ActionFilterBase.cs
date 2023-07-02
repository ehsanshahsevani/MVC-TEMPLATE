//using SampleResult;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Filters;

//namespace Infrastructure.Filters.ActionFilters.Base;

//public abstract class ActionFilterBase : IActionFilter
//{
//    public abstract void OnActionExecuted(ActionExecutedContext context);
//    public abstract void OnActionExecuting(ActionExecutingContext context);

//    // overload
//    // **************************************************
//    [NonAction]
//    protected IActionResult FluentResult<TResult>(FluentResults.Result<TResult> result)
//    {
//        var res = result.ConvertToSampleResult();

//        if (res.IsSuccess)
//        {
//            return new OkObjectResult(res);
//        }
//        else
//        {
//            return new BadRequestObjectResult(res);
//        }
//    }
//    // **************************************************

//    // **************************************************
//    [NonAction]
//    protected IActionResult FluentResult(FluentResults.Result result)
//    {
//        var res = result.ConvertToSampleResult();

//        if (res.IsSuccess)
//        {
//            return new OkObjectResult(res);
//        }
//        else
//        {
//            return new BadRequestObjectResult(res);
//        }
//    }
//    // **************************************************
//}
