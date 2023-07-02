using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Persistence.Settings
{
	public static class ExtensionServices
	{
		// migrations settings
		public static void AddConfigContext(this IServiceCollection services, string connectionString)
		{
			services.AddDbContext<Persistence.DatabaseContext>(opts =>
			{
				opts.UseSqlServer(connectionString);
			});
		}

		// identity micosoft settings
		public static void AddConfigIdentity(this IServiceCollection services)
		{
			services.AddAuthorizationCore(options =>
			{
				// ******************************************
				//options.AddPolicy(Utility.Constant.POLICY_ADMIN,
				//    policy => policy.RequireRole(Utility.Constant.ROLE_ADMIN));
				// ******************************************
			});

			IdentityBuilder builder =
				services.AddIdentity<Domain.User, Domain.Role>(option =>
				{
					option.Password.RequireDigit = false;
					option.Password.RequireLowercase = false;
					option.Password.RequireUppercase = false;
					option.Password.RequireNonAlphanumeric = false;
					option.Password.RequiredLength = 5;
					option.User.RequireUniqueEmail = true;

					option.SignIn.RequireConfirmedAccount = true;
					option.Lockout.MaxFailedAccessAttempts = 3;
					option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);

				}).AddRoles<Domain.Role>()
				.AddEntityFrameworkStores<Persistence.DatabaseContext>(); ;

			builder =
				new IdentityBuilder
					(builder.UserType, typeof(Domain.Role), builder.Services);

			builder.AddEntityFrameworkStores<DatabaseContext>()
				.AddDefaultTokenProviders();
		}
	}
}
