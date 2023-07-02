using ViewModels;
using Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Infrastructure.Filters.ActionFilters;

public class ValidationRoleFilter : IAsyncActionFilter // IActionFilter
{
	public IUnitOfWork UnitOfWork { get; }

	public ValidationRoleFilter(IUnitOfWork unitOfWork)
	{
		UnitOfWork = unitOfWork;
	}

	public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
	{
		var result = new FluentResults.Result();

		object? action = context.RouteData.Values["action"];
		object? controller = context.RouteData.Values["controller"];

		RoleViewModel? model =
			context.ActionArguments.FirstOrDefault
			  (current => current.Value is RoleViewModel).Value as RoleViewModel;

		if (model is null)
		{
			result.WithError(Resources.Messages.SystemError);
		}
		else
		{
			if (string.IsNullOrEmpty(model.Name) == true)
			{
				result.WithError(Resources.Messages.RoleNameRequired);
			}

			if (string.IsNullOrEmpty(model.Title) == true)
			{
				result.WithError(Resources.Messages.RoleTitleRequired);
			}
		}

		if (result.IsSuccess == true)
		{
			await next();
		}

		if (result.IsFailed == true)
		{
			context.Result = new BadRequestObjectResult(result);
		}
	}
}
