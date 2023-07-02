using Domain;
using AutoMapper;
using Persistence;
using WebSite.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace WebSite.Controllers;

public class HomeController
	: Infrastructure.ApplicationControllers.BaseSiteController
{
	public HomeController(IUnitOfWork unitOfWork, IMapper mapper,
		IConfiguration configuration, IWebHostEnvironment env, HttpClient httpClient,
		UserManager<User> userManager, RoleManager<Role> roleManager,
		SignInManager<User> signInManager, IHttpContextAccessor httpContextAccessor)
		: base(unitOfWork, mapper, configuration, env, httpClient,
			  userManager, roleManager, signInManager, httpContextAccessor)
	{
	}

	public IActionResult Index()
	{
		// ***********************************

		string x = "ehsan shahsevani";

		ViewData["FullName"] = x;

		// ***********************************

		// ***********************************

		string FullName = "ehsan shahsevani";

		ViewData[nameof(FullName)] = FullName;

		// ***********************************

		return View();
	}

	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public IActionResult Error()
	{
		return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
	}
}