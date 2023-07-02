using Domain;
using Utilities;
using ViewModels;
using Persistence;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Profiles;

public class MappingProfile : BaseProfile
{
	public MappingProfile
		(UserManager<User> userManager, RoleManager<Role> roleManager
		, IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork)
		: base(userManager, roleManager, httpContextAccessor, unitOfWork)
	{

		// Sample:
		// **********************************************************
		//CreateMap<Report, ReportViewModel>()

		//	.ForMember(vm => vm.CreateDate,
		//		option => option.MapFrom(model => ToShamsi(model.CreateDate)))

		//	.ForMember(vm => vm.DateOfOperation,
		//		option => option.MapFrom(model => ToShamsi(model.DateOfOperation)))

		//	.ReverseMap()

		//	.ForMember(model => model.CreateDate,
		//		option => option.MapFrom(vm => StringToDateTime(vm.CreateDate)))

		//	.ForMember(model => model.DateOfOperation,
		//		option => option.MapFrom(vm => StringToDateTime(vm.DateOfOperation)))

		//;
		// **********************************************************
	}

	public string? ToShamsi(DateTime? dateTime)
	{
		string? result = string.Empty;

		if (dateTime.HasValue == true)
		{
			result = dateTime.Value.ToShamsi(1);
		}

		return result;
	}

	public DateTime StringToDateTime(string? date)
	{
		var result =
			(date.StrigToDateTimeMiladi().HasValue == true)
				? date.StrigToDateTimeMiladi()!.Value : DateTime.Now;

		return result;
	}

}
