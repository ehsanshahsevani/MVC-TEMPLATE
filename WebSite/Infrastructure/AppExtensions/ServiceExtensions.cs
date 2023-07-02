using Models;
using System.Text;
using System.Data.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore; // Data Base
using Microsoft.AspNetCore.Identity; // Security
using Microsoft.IdentityModel.Tokens; // JWT
using Microsoft.Extensions.Configuration;
// using Microsoft.AspNetCore.Mvc.Versioning; // set Version For Controller
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer; // JWT

namespace Infrastructure.Extensions;

public static class ServiceExtensions
{
	public static void ConfigureCors(this IServiceCollection service)
		=>
		service.AddCors(option =>
		{
			option.AddPolicy("CorsPolicy", builder =>
			builder.AllowAnyOrigin()    // WithOrgin("https://shahsevaniehsan.ir")
				   .AllowAnyMethod()    // WithMethodes("get;", Post)
				   .AllowAnyHeader()    // WithHeaders("accept", "contenttype")
				   );
		});

	public static void ConfigureIISIntegration(this IServiceCollection services)
		=>
		services.Configure<IISOptions>(option =>
		{
		});

	// configure Unit Of Work
	public static void ConfigureUnitOfWork(this IServiceCollection services,
		IConfiguration configuration)
	{
		services.AddScoped<Persistence.IUnitOfWork, Persistence.UnitOfWork>(sp =>
		{
			string connectionString =
						configuration.GetSection(key: "ConnectionStrings")
						.GetSection(key: "LocalConnection").Value!;

			Persistence.Tools.Options options =
				new Persistence.Tools.Options(connectionString);

			Persistence.UnitOfWork unitOfWork = new Persistence.UnitOfWork(options: options);

			return unitOfWork;
		});
	}

	//public static void ConfigureJWt(this IServiceCollection services, IConfiguration configuration)
	//{
	//	var settings = new ApplicationSettings.JwtSettings();
	//	configuration.GetSection(nameof(ApplicationSettings.JwtSettings)).Bind(settings);

	//	byte[] key =
	//		Encoding.ASCII.GetBytes(s: settings.SecretKey!);

	//	services.AddAuthentication(option =>
	//	{
	//		option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	//		option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
	//	})
	//	.AddJwtBearer(option =>
	//	{
	//		option.RequireHttpsMetadata = true;
	//		option.SaveToken = true;
	//		option.TokenValidationParameters = new TokenValidationParameters
	//		{
	//			ValidateIssuer = false,
	//			ValidateAudience = false,
	//			ValidateIssuerSigningKey = true,

	//			IssuerSigningKey =
	//					new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key: key),

	//			ClockSkew = System.TimeSpan.Zero,


	//			//ValidateIssuer = false,
	//			//ValidateAudience = false,
	//			//ValidateLifetime = true,
	//			//ValidateIssuerSigningKey = true,
	//			//// ValidIssuer = jwtSettings.GetSection("validIssuer").Value,
	//			//// ValidAudience = jwtSettings.GetSection("validAudience").Value,
	//		};
	//	});
	//}

}
