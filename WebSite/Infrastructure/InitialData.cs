using Domain;
using Resources;
using Persistence;
using System.Drawing;
using System.Xml.Linq;
using System.Resources;
using System.Globalization;
using Microsoft.AspNetCore.Server.IISIntegration;

namespace Infrastructure;

public class InitialData<T> : object
{
	#region Settings
	public InitialData(
		Microsoft.AspNetCore.Identity.UserManager<Domain.User>? userManager,
		Microsoft.AspNetCore.Identity.RoleManager<Domain.Role>? roleManager, IConfiguration configuration,
		Persistence.IUnitOfWork? unitOfWork) : base()
	{
		UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
		UserManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
		RoleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
		Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
	}

	protected Microsoft.AspNetCore.Identity.UserManager<Domain.User> UserManager { get; }
	protected Microsoft.AspNetCore.Identity.RoleManager<Domain.Role> RoleManager { get; }
	protected IConfiguration Configuration { get; }
	protected IUnitOfWork UnitOfWork { get; }
	#endregion

	/// <summary>
	/// نقش های پیش فرض سیستم
	/// </summary>
	/// <returns></returns>
	public async Task CreateRolesAsync()
	{
		var roles = GetValuesFromResources<Resources.InitialData.Roles>();

		if (roles is null)
		{
			throw new ArgumentNullException(nameof(roles));
		}

		foreach (var item in roles)
		{
			var searchRole = await RoleManager.FindByNameAsync(item.Key);

			if (searchRole is not null)
			{
				searchRole.Title = item.Value;
				await RoleManager.UpdateAsync(searchRole);
				continue;
			}

			var role = new Role()
			{
				Name = item.Key,
				Title = item.Value,
			};

			var resultRole = await RoleManager.CreateAsync(role);

			if (resultRole.Succeeded == false)
			{
				foreach (var identityError in resultRole.Errors.ToList())
				{
					// Logger.LogError();
				}
			}
		}
	}

	/// <summary>
	/// عناوین سطوح دسترسی
	/// </summary>
	/// <returns></returns>
	//public async Task CreatePermissionTitleAsync()
	//{
	//	var PermissionTitles = GetValuesFromResources<Resources.InitialData.PermissionTitles>();

	//	if (PermissionTitles is null)
	//	{
	//		throw new ArgumentNullException(nameof(PermissionTitles));
	//	}

	//	foreach (var title in PermissionTitles)
	//	{
	//		if (title.Value is null)
	//		{
	//			continue;
	//		}

	//		var searchPermissionTitle =
	//			await UnitOfWork.PermissionTitleRepository.FindByNameAsync(title.Value);

	//		if (searchPermissionTitle is not null)
	//		{
	//			continue;
	//		}

	//		PermissionTitle permissionTitle = new PermissionTitle()
	//		{
	//			Title = title.Value!,
	//		};
	//		await UnitOfWork.PermissionTitleRepository.AddAsync(permissionTitle);
	//	}
	//	await UnitOfWork.SendToDatabaseAsync();
	//}

	/// <summary>
	/// وضعیت تیکت
	/// ثبت و در صورت وجود هر شماره ترتیب ویرایش صورت میگیرد
	/// </summary>
	/// <returns></returns>
	//public async Task CreateStatusTicketsAsync()
	//{
	//	var statusTickets = GetValuesFromResources<Resources.InitialData.StatusTickets>();

	//	if (statusTickets is null)
	//	{
	//		throw new ArgumentNullException(nameof(statusTickets));
	//	}

	//	foreach (var title in statusTickets)
	//	{
	//		if (title.Value is null)
	//		{
	//			continue;
	//		}

	//		var searchByOrder =
	//			await UnitOfWork.StatusTicketRepository.FindStatusTicketByOrder(int.Parse(title.Key));

	//		if (searchByOrder is null)
	//		{
	//			StatusTicket statusTicket = new StatusTicket
	//			{
	//				Name = title.Value,
	//				Order = int.Parse(title.Key)
	//			};

	//			await UnitOfWork.StatusTicketRepository.AddAsync(statusTicket);
	//		}
	//		else
	//		{
	//			searchByOrder!.Name = title.Value;

	//			await UnitOfWork.StatusTicketRepository.UpdateAsync(searchByOrder);
	//		}

	//	}
	//	await UnitOfWork.SendToDatabaseAsync();
	//}

	/// <summary>
	/// نوع تیکت ها
	/// آپدیت در صورتی که عنوان یک ترتیب تغییر کرده باشد
	/// </summary>
	/// <returns></returns>
	//public async Task CreateTicketTypeAsync()
	//{
	//	var ticketTypes = GetValuesFromResources<Resources.InitialData.TicketTypes>();

	//	if (ticketTypes is null)
	//	{
	//		throw new ArgumentNullException(nameof(ticketTypes));
	//	}

	//	foreach (var title in ticketTypes)
	//	{
	//		if (title.Value is null)
	//		{
	//			continue;
	//		}

	//		var searchByOrder =
	//			await UnitOfWork.TicketTypeReposiory.FindByOrder(int.Parse(title.Key));

	//		if (searchByOrder is null)
	//		{
	//			TicketType statusTicket = new TicketType
	//			{
	//				Name = title.Value,
	//				Order = int.Parse(title.Key)
	//			};

	//			await UnitOfWork.TicketTypeReposiory.AddAsync(statusTicket);
	//		}
	//		else
	//		{
	//			searchByOrder!.Name = title.Value;

	//			await UnitOfWork.TicketTypeReposiory.UpdateAsync(searchByOrder);
	//		}

	//	}
	//	await UnitOfWork.SendToDatabaseAsync();
	//}

	/// <summary>
	/// اولویت کاربران
	/// </summary>
	/// <returns></returns>
	//public async Task CreateUserPriotryAsync()
	//{
	//	var UserPriotries = GetValuesFromResources<Resources.InitialData.UserPriotries>();

	//	if (UserPriotries is null)
	//	{
	//		throw new ArgumentNullException(nameof(UserPriotries));
	//	}

	//	foreach (var title in UserPriotries)
	//	{
	//		if (title.Value is null)
	//		{
	//			continue;
	//		}

	//		var searchByOrder =
	//			await UnitOfWork.UserPriotryRepository.FindByOrder(int.Parse(title.Key));

	//		if (searchByOrder is null)
	//		{
	//			UserPriotry userPriotry = new UserPriotry
	//			{
	//				Name = title.Value,
	//				Order = int.Parse(title.Key)
	//			};

	//			await UnitOfWork.UserPriotryRepository.AddAsync(userPriotry);
	//		}
	//		else
	//		{
	//			searchByOrder!.Name = title.Value;

	//			await UnitOfWork.UserPriotryRepository.UpdateAsync(searchByOrder);
	//		}

	//	}
	//	await UnitOfWork.SendToDatabaseAsync();
	//}

	/// <summary>
	/// نوع ارجاع
	/// </summary>
	/// <returns></returns>
	//public async Task CreateReferenceTypesAsync()
	//{
	//	var referenceTypes = GetValuesFromResources<Resources.InitialData.ReferenceTypes>();

	//	if (referenceTypes is null)
	//	{
	//		throw new ArgumentNullException(nameof(referenceTypes));
	//	}

	//	foreach (var title in referenceTypes)
	//	{
	//		if (title.Value is null)
	//		{
	//			continue;
	//		}

	//		var searchByOrder =
	//			await UnitOfWork.ReferencesTypeRepository.FindByOrder(int.Parse(title.Key));

	//		if (searchByOrder is null)
	//		{
	//			ReferencesType userPriotry = new ReferencesType
	//			{
	//				Name = title.Value,
	//				Order = int.Parse(title.Key)
	//			};

	//			await UnitOfWork.ReferencesTypeRepository.AddAsync(userPriotry);
	//		}
	//		else
	//		{
	//			searchByOrder!.Name = title.Value;

	//			await UnitOfWork.ReferencesTypeRepository.UpdateAsync(searchByOrder);
	//		}

	//	}
	//	await UnitOfWork.SendToDatabaseAsync();
	//}

	/// <summary>
	/// صفحات
	/// </summary>
	/// <returns></returns>
	//public async Task CreatePagesAsync()
	//{
	//	var referenceTypes = GetValuesFromResources<Resources.InitialData.Pages>();

	//	if (referenceTypes is null)
	//	{
	//		throw new ArgumentNullException(nameof(referenceTypes));
	//	}

	//	foreach (var title in referenceTypes)
	//	{
	//		if (title.Value is null)
	//		{
	//			continue;
	//		}

	//		var searchByHref =
	//			await UnitOfWork.PageRepository.FindByHrefAsync(title.Key);

	//		var icon =
	//			GetValueFromResourcesByKey<Resources.InitialData.PageIcon>(title.Key);

	//		var order =
	//			Convert.ToInt32
	//				(GetValueFromResourcesByKey
	//				<Resources.InitialData.PageOrder>(title.Key));

	//		if (searchByHref is null)
	//		{

	//			Page page = new Page
	//			{
	//				Href = title.Key,
	//				Name = title.Value,

	//				CssIcone = icon,

	//				Order = order,

	//				IsDefault = false,
	//				IsDeleted = false,

	//				ExpireDate = null,
	//				CreateDate = DateTime.Now,
	//			};

	//			await UnitOfWork.PageRepository.AddAsync(page);
	//		}
	//		else
	//		{
	//			searchByHref!.Name = title.Value;
	//			searchByHref!.CssIcone = icon;
	//			searchByHref!.Order = order;
	//			searchByHref!.IsDefault = false;
	//			searchByHref!.IsDeleted = false;
	//			searchByHref!.ExpireDate = null;
	//			searchByHref!.CreateDate = DateTime.Now;

	//			await UnitOfWork.PageRepository.UpdateAsync(searchByHref);
	//		}

	//	}
	//	await UnitOfWork.SendToDatabaseAsync();
	//}

	/// <summary>
	/// سطح دسترسی ها
	/// </summary>
	/// <returns></returns>
	//public async Task CreatePermmisions()
	//{
	//	List<string> Admin = new()
	//	{
	//		nameof(Resources.InitialData.Pages.Users),
	//		nameof(Resources.InitialData.Pages.Ticket),
	//		nameof(Resources.InitialData.Pages.ReceivedTickets),
	//		nameof(Resources.InitialData.Pages.ShowtTicket),
	//		nameof(Resources.InitialData.Pages.SaveNewTicket),
	//		nameof(Resources.InitialData.Pages.Customer),
	//		nameof(Resources.InitialData.Pages.GroupManagment),
	//		nameof(Resources.InitialData.Pages.GroupRelations),
	//		nameof(Resources.InitialData.Pages.CustomerProject),
	//		nameof(Resources.InitialData.Pages.TicketReference),
	//		nameof(Resources.InitialData.Pages.Gallery),
	//		nameof(Resources.InitialData.Pages.TicketTrending),
	//		nameof(Resources.InitialData.Pages.Profile),
	//		nameof(Resources.InitialData.Pages.FAQCreate),
	//		nameof(Resources.InitialData.Pages.FAQ),
	//		nameof(Resources.InitialData.Pages.FAQCategories),
	//		nameof(Resources.InitialData.Pages.SubSystem),
	//		nameof(Resources.InitialData.Pages.Project),
	//		nameof(Resources.InitialData.Pages.Reports),
	//		nameof(Resources.InitialData.Pages.ActionType),
	//		nameof(Resources.InitialData.Pages.ActionPlace),
	//	};

	//	List<string> AdminCompany = new()
	//	{
	//		nameof(Resources.InitialData.Pages.Users),
	//		nameof(Resources.InitialData.Pages.Ticket),
	//		nameof(Resources.InitialData.Pages.ReceivedTickets),
	//		nameof(Resources.InitialData.Pages.ShowtTicket),
	//		nameof(Resources.InitialData.Pages.SaveNewTicket),
	//		nameof(Resources.InitialData.Pages.GroupManagment),
	//		nameof(Resources.InitialData.Pages.GroupRelations),
	//		nameof(Resources.InitialData.Pages.Gallery),
	//		nameof(Resources.InitialData.Pages.TicketTrending),
	//		nameof(Resources.InitialData.Pages.Profile),
	//		nameof(Resources.InitialData.Pages.FAQ),
	//		nameof(Resources.InitialData.Pages.Reports),
	//	};

	//	List<string> User = new()
	//	{
	//		nameof(Resources.InitialData.Pages.Ticket),
	//		nameof(Resources.InitialData.Pages.ReceivedTickets),
	//		nameof(Resources.InitialData.Pages.ShowtTicket),
	//		nameof(Resources.InitialData.Pages.SaveNewTicket),
	//		nameof(Resources.InitialData.Pages.Gallery),
	//		nameof(Resources.InitialData.Pages.Profile),
	//		nameof(Resources.InitialData.Pages.FAQ),
	//		nameof(Resources.InitialData.Pages.Reports),
	//	};

	//	var listPermissions = new List<Permission>();

	//	var roleAdmin = await RoleManager.FindByNameAsync(nameof(Admin));

	//	var permmisionTitle = await UnitOfWork
	//		.PermissionTitleRepository.FindByNameAsync(Resources.InitialData.PermissionTitles.All);

	//	if (roleAdmin != null && permmisionTitle != null)
	//	{
	//		foreach (var item in Admin)
	//		{
	//			var page = await UnitOfWork.PageRepository.FindByHrefAsync(item);
	//			if (page != null)
	//			{
	//				Permission permission = new()
	//				{
	//					CreateDate = DateTime.Now,
	//					IsDeleted = false,
	//					PageID = page.Id,
	//					RoleID = roleAdmin.Id,
	//					permissionTitleID = permmisionTitle.Id
	//				};

	//				var isExist = await UnitOfWork.PermissionRepository.IsExistAsync(permission);
	//				if (isExist == false)
	//				{
	//					listPermissions.Add(permission);
	//				}
	//			}
	//		}
	//	}

	//	// -------------------------
	//	var roleAdminCompany = await RoleManager.FindByNameAsync(nameof(AdminCompany));

	//	if (roleAdminCompany != null && permmisionTitle != null)
	//	{
	//		foreach (var item in AdminCompany)
	//		{
	//			var page = await UnitOfWork.PageRepository.FindByHrefAsync(item);
	//			if (page != null)
	//			{
	//				Permission permission = new()
	//				{
	//					CreateDate = DateTime.Now,
	//					IsDeleted = false,
	//					PageID = page.Id,
	//					RoleID = roleAdminCompany.Id,
	//					permissionTitleID = permmisionTitle.Id
	//				};

	//				var isExist = await UnitOfWork.PermissionRepository.IsExistAsync(permission);
	//				if (isExist == false)
	//				{
	//					listPermissions.Add(permission);
	//				}
	//			}
	//		}
	//	}
	//	// ----------------------------
	//	var roleUser = await RoleManager.FindByNameAsync(nameof(User));

	//	if (roleUser != null && permmisionTitle != null)
	//	{
	//		foreach (var item in User)
	//		{
	//			var page = await UnitOfWork.PageRepository.FindByHrefAsync(item);
	//			if (page != null)
	//			{
	//				Permission permission = new()
	//				{
	//					CreateDate = DateTime.Now,
	//					IsDeleted = false,
	//					PageID = page.Id,
	//					RoleID = roleUser.Id,
	//					permissionTitleID = permmisionTitle.Id
	//				};

	//				var isExist = await UnitOfWork.PermissionRepository.IsExistAsync(permission);
	//				if (isExist == false)
	//				{
	//					listPermissions.Add(permission);
	//				}
	//			}
	//		}
	//	}
	//	// ----------------------------

	//	await UnitOfWork.PermissionRepository.AddRangeAsync(listPermissions);
	//	await UnitOfWork.SendToDatabaseAsync();
	//}

	public async Task CreateCustomerAndUserSystem()
	{
		var user = new User()
		{
			FirstName = "ادمین",
			LastName = "ادمین",
			PhoneNumber = "000000000000",
			CreateDate = DateTime.Now,
			IsDeleted = false,
			UserName = "admin"
		};

		var searchUser = await UserManager.FindByNameAsync("admin");
		if (searchUser is null)
		{
			await UserManager.CreateAsync(user, "0123456789");
			await UserManager.AddToRoleAsync(user, nameof(Resources.InitialData.Roles.Admin));
		}
	}

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
	public async Task RunQuery()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
	{
		// await UnitOfWork.ServerLogRepository.DeleteFromTableServerLog();
	}

	// **************************************************
	private string? GetValueFromResourcesByKey<TResource>(string key)
	{
		ResourceManager resources = new ResourceManager(typeof(TResource));

		ResourceSet? resourceSet =
			resources.GetResourceSet(CultureInfo.CurrentUICulture, true, true);

		if (resourceSet is null)
		{
			throw new ArgumentNullException(nameof(resourceSet));
		}

		string? value = resourceSet.GetString(key);

		return value;
	}
	// **************************************************

	// **************************************************
	private Dictionary<string, string?>? GetValuesFromResources<TResource>()
	{
		var result = new Dictionary<string, string?>();

		ResourceManager resource = new ResourceManager(typeof(TResource));

		ResourceSet? resourceSet =
			resource.GetResourceSet(CultureInfo.CurrentUICulture, true, true);

		if (resourceSet is null)
		{
			throw new ArgumentNullException(nameof(resourceSet));
		}

		var dictionaryEnumerator = resourceSet.GetEnumerator();

		if (dictionaryEnumerator is null)
		{
			throw new ArgumentNullException(nameof(dictionaryEnumerator));
		}

		while (dictionaryEnumerator.MoveNext() == true)
		{
			var value =
				dictionaryEnumerator.Value is null ? "" : (string)dictionaryEnumerator.Value;

			result.Add((string)dictionaryEnumerator.Key, value);
		}

		return result;
	}
	// **************************************************

}
