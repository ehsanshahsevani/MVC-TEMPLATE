using Persistence.Settings;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

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

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCors("CorsPolicy");

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
