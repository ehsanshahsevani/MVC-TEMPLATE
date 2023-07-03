namespace Infrastructure.Middlewares
{
	public static class ExtensionMethods
	{
		static ExtensionMethods()
		{
		}

		public static Microsoft.AspNetCore.Builder.IApplicationBuilder
			UseGlobalException(this Microsoft.AspNetCore.Builder.IApplicationBuilder app)
		{
			// UseMiddleware -> //using Microsoft.AspNetCore.Builder;
			return app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
		}
	}
}
