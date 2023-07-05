using Domain;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Infrastructure.Middlewares
{
	public class GlobalExceptionHandlerMiddleware : object
	{
		public GlobalExceptionHandlerMiddleware(RequestDelegate next) : base()
		{
			Next = next;
		}

		private RequestDelegate Next { get; }

		public async Task InvokeAsync
			(HttpContext httpContext, Persistence.IUnitOfWork unitOfWork)
		{
			try
			{
				await Next(httpContext);
			}
			catch (Exception ex)
			{
				#region Log To DataBase ...
				System.Text.StringBuilder stringBuilder = new();

				stringBuilder.Append($"{nameof(httpContext.Request.Path)}: {httpContext.Request.Path}");
				stringBuilder.Append($" - ");
				stringBuilder.Append($"{nameof(httpContext.Request.Method)} :  {httpContext.Request.Method}");
				stringBuilder.Append($" - ");
				stringBuilder.Append
					($"{nameof(ex.Message)}: {ex.Message}");

				var message = stringBuilder.ToString();

				ControllerActionDescriptor?
					controllerActionDescriptor =
						httpContext.GetEndpoint()?.Metadata
							.GetMetadata<ControllerActionDescriptor>();

				ServerLog serverLog = new()
				{
					Message = message,
					IsDeleted = false,
					CreateDate = DateTime.Now,
					Exceptions = ex.ToString(),
					RequestPath = httpContext.Request.Path,

					Description =
						$"controller:{controllerActionDescriptor?.ControllerName} - action: {controllerActionDescriptor?.ActionName}",

					MethodName = controllerActionDescriptor?.ActionName,

					ClassName = controllerActionDescriptor?.ControllerName,

					Namespace = controllerActionDescriptor?.DisplayName,

					RemoteIP = httpContext.Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
					PortIP = httpContext.Request.HttpContext.Connection.RemotePort.ToString(),
					HttpReferrer = httpContext.Request.Headers["Referer"].ToString(),
				};

				await unitOfWork!.ServerLogRepository.AddAsync(serverLog);
				await unitOfWork.SendToDatabaseAsync();
				#endregion

				httpContext.Response.Redirect
					(location: "/Home/Error", permanent: false);
			}
		}
	}
}
