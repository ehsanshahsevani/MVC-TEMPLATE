//using SampleResult;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Filters;

//namespace Infrastructure.Filters.ActionFilters.Base;

//public abstract class ActionFilterBaseAsync : IAsyncActionFilter
//{
//    public abstract Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next);

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
