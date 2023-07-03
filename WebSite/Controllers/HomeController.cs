using Domain;
using AutoMapper;
using Persistence;
using WebSite.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

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

	public async Task<IActionResult> Index()
	{
		// ***********************************

		string x = "Test Data Number 1";

		ViewData["Test"] = x;

		// ***********************************

		// ***********************************

		string Test = "Test Data";

		ViewData[nameof(Test)] = Test;

		// ***********************************

		// ***********************************
		// Test Database

		var post = new Post
		{
			IsActive = true,
			IsDeleted = false,
			Title = "Title Test",
			Text = "Text Test",
			Description = "Description Test",
		};

		var anyPost = await UnitOfWork.PostRepository.GetAllAsync();

		if (anyPost.Count() == 0)
		{
			await UnitOfWork.PostRepository.AddAsync(post);
			await SaveAsync();
		}
		// ***********************************

		return View();
	}

	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public IActionResult Error()
	{
		return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
	}
}