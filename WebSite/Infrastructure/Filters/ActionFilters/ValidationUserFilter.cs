using Domain;
using ViewModels;
using Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Utilities;

namespace Infrastructure.Filters.ActionFilters;

public class ValidationUserFilter : IAsyncActionFilter
{
	public IUnitOfWork UnitOfWork { get; }
	public UserManager<User> UserManager { get; }
	public RoleManager<Role> RoleManager { get; }

	public ValidationUserFilter(IUnitOfWork unitOfWork, UserManager<User> userManager, RoleManager<Role> roleManager)
	{
		UnitOfWork = unitOfWork;
		UserManager = userManager;
		RoleManager = roleManager;
	}

	public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
	{
		var result = new FluentResults.Result();

		object? action = context.RouteData.Values["action"];
		object? controller = context.RouteData.Values["controller"];

		//UserViewModel? model =
		//	context.ActionArguments.FirstOrDefault
		//	  (current => current.Value is UserViewModel).Value as UserViewModel;

		//if (model is null)
		//{
		//	result.WithError(Resources.Messages.SystemError);
		//}
		//else
		//{
		//	if (string.IsNullOrEmpty(model.FirstName) == true)
		//	{
		//		result.WithError(Resources.Messages.NameRequired);
		//	}
		//}

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
