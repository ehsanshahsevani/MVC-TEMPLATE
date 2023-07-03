using Persistence.Settings;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;
using Infrastructure.Middlewares;

var webApplicationOptions =
	new WebApplicationOptions
	{
		EnvironmentName =
			System.Diagnostics.Debugger.IsAttached
				? Environments.Development : Environments.Production,
	};

var builder = WebApplication.CreateBuilder(options: webApplicationOptions);

// Add services to the container.
builder.Services.AddControllersWithViews()
		.AddJsonOptions(option =>
		{
			option.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
		})
	;

// infrastracture services
builder.Services.ConfigureCors();

var connectionString =
	builder.Configuration.GetConnectionString("LocalConnection")!;

builder.Services.AddDatabaseContextEntityFrameworkCore(connectionString);

builder.Services.ConfigureUnitOfWork(builder.Configuration);

builder.Services.AddIdentityMicrosoft();

builder.Services.AddHttpClient();

// ******** 
// add auto mapper
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped(
	provider => new AutoMapper.MapperConfiguration(cfg =>
	{
#pragma warning disable CS8604 // Possible null reference argument.
		cfg.AddProfile(new Infrastructure.Profiles.MappingProfile
			(provider.GetService<UserManager<Domain.User>>(),
			provider.GetService<RoleManager<Domain.Role>>(),
			provider.GetService<IHttpContextAccessor>(),
			provider.GetService<Persistence.IUnitOfWork>()));
#pragma warning restore CS8604 // Possible null reference argument.
	})
		.CreateMapper());
// ********

var app = builder.Build();

// app.ConfigureExceptionHandler();

// IsDevelopment() -> using Microsoft.Extensions.Hosting;
if (app.Environment.IsDevelopment())
{
	// **************************************************
	// UseDeveloperExceptionPage() -> using Microsoft.AspNetCore.Builder;
	app.UseDeveloperExceptionPage();
	// **************************************************
}
else
{
	// **************************************************
	// UseGlobalException() -> using Infrastructure.Middlewares;
	app.UseGlobalException();
	// **************************************************

	// **************************************************
	// UseExceptionHandler() -> using Microsoft.AspNetCore.Builder;
	app.UseExceptionHandler("/Errors/Error");
	// **************************************************

	// **************************************************
	// The default HSTS value is 30 days.
	// You may want to change this for production scenarios,
	// see https://aka.ms/aspnetcore-hsts
	// UseHsts() -> using Microsoft.AspNetCore.Builder; 
	app.UseHsts();
	// **************************************************
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCors("CorsPolicy");

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
