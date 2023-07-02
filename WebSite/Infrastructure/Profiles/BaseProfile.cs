using Domain;
using Microsoft.AspNetCore.Identity;
using Persistence;
using System.Text;

namespace Infrastructure.Profiles;

public abstract class BaseProfile : AutoMapper.Profile
{
	public UserManager<User> UserManager { get; }
	public RoleManager<Role> RoleManager { get; }
	public IWebHostEnvironment? HostEnvironment { get; }
	public IUnitOfWork UnitOfWork { get; }

	/// <summary>
	/// آدرس فعلی دامین - سایت
	/// </summary>
	public string SiteAddress { get; set; }

	public BaseProfile(UserManager<User> userManager, RoleManager<Role> roleManager,
		IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork)
	{
		SiteAddress =
			$"{httpContextAccessor.HttpContext?.Request.Scheme}://{httpContextAccessor.HttpContext?.Request.Host}";

		UserManager = userManager;
		RoleManager = roleManager;
		UnitOfWork = unitOfWork;
	}

	public string? CheckAndGetString(params string?[] values)
	{
		StringBuilder stringBuilder = new StringBuilder();

		if (values.Any(val => string.IsNullOrEmpty(val) == false) == false)
		{
			return null;
		}

		foreach (var item in values)
		{
			if (string.IsNullOrEmpty(item) == false)
			{
				stringBuilder.Append($"{item} ");
			}
		}

		return stringBuilder.ToString();
	}

}
