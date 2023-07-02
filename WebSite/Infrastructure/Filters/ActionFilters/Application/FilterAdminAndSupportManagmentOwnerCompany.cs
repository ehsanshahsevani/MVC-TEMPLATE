using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Infrastructure.Filters.ActionFilters.Application;

public class FilterAdminAndSupportManagmentOwnerCompany : IAsyncActionFilter
{
	public FilterAdminAndSupportManagmentOwnerCompany
		(UserManager<User> userManager, IConfiguration configuration)
	{
		UserManager = userManager;
		Configuration = configuration;
	}

	public UserManager<User> UserManager { get; }
	public IConfiguration Configuration { get; }

	public async Task OnActionExecutionAsync
		(ActionExecutingContext context, ActionExecutionDelegate next)
	{
		var result = new FluentResults.Result();

		string key = Configuration.GetSection("JwtSettings").GetValue<string>("SecretKey")!;

		var tokenDetails = JWT.JwtUtility.GetTokenDetail(context.HttpContext, key);

		if (tokenDetails.TokenIsOk == false)
		{
			result.WithError(Resources.Messages.NotAccessToPageConnectToCompany);
		}

		// admin -> true

		// join in group support managment in owner company -> true

		if (result.IsSuccess == true)
		{
			await next();
		}

		if (result.IsFailed == true)
		{
			context.Result =
				new BadRequestObjectResult(result);
		}
	}
}
