using Domain;
using System.Net;
using Persistence;
using Microsoft.AspNetCore.Diagnostics;

namespace Infrastructure.Middelwares;

public static class ExceptionMiddelware
{
	public static void ConfigureExceptionHandler(this WebApplication app)
	{
		// Infrastructure -> Middelwares
		// Infrastructure.Middelwares.ExceptionMiddelware.ConfigureExceptionHandler(app, logger);


		//if (unitOfWork is null)
		//{
		//    throw new ArgumentNullException(nameof(unitOfWork));
		//}

		app.UseExceptionHandler(appError =>
		{
			appError.Run(async context =>
			{
				using var scope = app.Services.CreateScope();

				var unitOfWork =
					scope.ServiceProvider.GetService(typeof(IUnitOfWork)) as IUnitOfWork;

				context.Response.StatusCode =
					(int)HttpStatusCode.InternalServerError;

				context.Response.ContentType = "application/json";

				IExceptionHandlerFeature? contextFeature =
					context.Features.Get<IExceptionHandlerFeature>();

				string? path = context.Request.Path.Value;
				string methode = context.Request.Method;

				if (contextFeature != null)
				{
					System.Text.StringBuilder stringBuilder = new();

					stringBuilder.Append($"{nameof(path)}: {path}");
					stringBuilder.Append($" - ");
					stringBuilder.Append($"{nameof(methode)}: {methode}");
					stringBuilder.Append($" - ");
					stringBuilder.Append
						($"{nameof(contextFeature.Error)}: {contextFeature.Error}");

					var message = stringBuilder.ToString();

					dynamic features = context.Features;
					var featuresVar = context.Features;

					var serverLog = new ServerLog()
					{
						IsDeleted = false,
						CreateDate = DateTime.Now,
						Exceptions = contextFeature.Error.ToString(),
						Message = message,
						RequestPath = path,
						Description = $"controller: {contextFeature.RouteValues?["controller"]} - action: {contextFeature.RouteValues?["action"]}",
						MethodName = contextFeature.RouteValues?["action"]?.ToString(),
						ClassName = contextFeature.RouteValues?["controller"]?.ToString(),
						Namespace = contextFeature.Endpoint?.DisplayName,
						RemoteIP = context.Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
						PortIP = context.Request.HttpContext.Connection.RemotePort.ToString(),
						HttpReferrer = context.Request.Headers["Referer"].ToString(),
					};

					await unitOfWork!.ServerLogRepository.AddAsync(serverLog);

					if (message.Contains("IDX10223: Lifetime validation failed.") == true)
					{
						await context.Response.WriteAsync(new Models.ErrorModel.ErrorDetails
						{
							StatusCode = context.Response.StatusCode,
							Message = "LifeTimeError",
						}.ToString());
					}
					else
					{
						await unitOfWork.SendToDatabaseAsync();

						await context.Response.WriteAsync(new Models.ErrorModel.ErrorDetails
						{
							StatusCode = context.Response.StatusCode,
							Message = "Internal Server Error.",
						}.ToString());
					}
				}
			});
		});
	}
}