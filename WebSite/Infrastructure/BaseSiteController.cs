using Domain;
using Utilities;
using AutoMapper;
using Persistence;
using Infrastructure.JWT;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure;

public abstract class BaseSiteController : Controller
{
	#region Constructor AND Property
	public BaseSiteController(IUnitOfWork unitOfWork,
		IMapper mapper,
		IConfiguration configuration,
		IWebHostEnvironment env, HttpClient httpClient,
		UserManager<User> userManager,
		RoleManager<Role> roleManager,
		SignInManager<User> signInManager,
		IHttpContextAccessor httpContextAccessor)
	{
		Mapper = mapper;
		UnitOfWork = unitOfWork;

		Env = env;
		HttpClient = httpClient;
		Configuration = configuration;

		UserManager = userManager;
		RoleManager = roleManager;
		SignInManager = signInManager;

		HttpContextAccessor = httpContextAccessor;

		if (httpContextAccessor is not null)
		{
			SiteAddress =
				$"{httpContextAccessor.HttpContext?.Request.Scheme}://{httpContextAccessor.HttpContext?.Request.Host}";
		}
		else
		{
			SiteAddress = string.Empty;
		}
	}

	// **************************************************
	// libs
	public IUnitOfWork UnitOfWork { get; }
	public IMapper Mapper { get; }
	// **************************************************

	// **************************************************
	// microsoft
	public IConfiguration Configuration { get; }
	public IWebHostEnvironment Env { get; }
	public HttpClient HttpClient { get; }
	// **************************************************

	// **************************************************
	// identity
	protected UserManager<User> UserManager { get; }
	protected RoleManager<Role> RoleManager { get; }
	protected SignInManager<User> SignInManager { get; }
	public IHttpContextAccessor HttpContextAccessor { get; }

	// **************************************************

	// **************************************************
	protected string SiteAddress { get; }
	// **************************************************
	#endregion

	#region Functions
	// overload
	// **************************************************
	//[NonAction]
	//protected IActionResult FluentResult<TResult>(FluentResults.Result<TResult> result)
	//{
	//	var res = result.ConvertToSampleResult();

	//	if (res.IsSuccess)
	//	{
	//		return Ok(res);
	//	}
	//	else
	//	{
	//		return BadRequest(res);
	//	}
	//}
	// **************************************************

	// **************************************************
	//[NonAction]
	//protected IActionResult FluentResult(FluentResults.Result result)
	//{
	//	var res = result.ConvertToSampleResult();

	//	if (res.IsSuccess)
	//	{
	//		return Ok(res);
	//	}
	//	else
	//	{
	//		return BadRequest(res);
	//	}
	//}
	// **************************************************

	// **************************************************
	[NonAction]
	public async Task SaveAsync()
	{
		var date = DateTime.Now;

		var detailsLog = new DetailsLog()
		{
			InsertDateTime = date,

			UserName = HttpContextAccessor.HttpContext?.User.Identity?.Name,

			UserId = Convert.ToInt32
				(HttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value),

			RemoteIP = HttpContext.Connection.RemoteIpAddress?.ToString(),

			PortIP = HttpContext.Connection.RemotePort.ToString(),

			HttpReferrer = HttpContext.Request.Headers["Referer"].ToString(),

			IsDeleted = false,

			RequestPath = HttpContext.Request.Path,

			Description = $"{date.ToShamsi(1)} - {date.ToString("HH:mm:ss")} - {date.ToShamsi()}",
		};

		await UnitOfWork.SaveAsync(detailsLog);
	}
	// **************************************************

	// **************************************************
	protected TokenDetails GetTokenDetail()
	{
		string key = Configuration.GetSection("JwtSettings").GetValue<string>("SecretKey")!;

		var tokenDetails = JwtUtility.GetTokenDetail(HttpContext, key);

		return tokenDetails;
	}
	// **************************************************
	#endregion
}
