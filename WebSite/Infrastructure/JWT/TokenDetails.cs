namespace Infrastructure.JWT;

public class TokenDetails : object
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	public TokenDetails() : base()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	{
		TokenIsOk = false;
		RoleNames = new List<string>();
		PageHrefs = new List<string>();
	}

	public string? Token { get; set; }

	public int UserId { get; set; }
	public int CustomerId { get; set; }
	public string UserName { get; set; }

	public bool IsOwner { get; set; }

	public List<string> RoleNames { get; set; }
	public List<string> PageHrefs { get; set; }

	public bool TokenIsOk { get; set; }

	public bool IsAdmin()
	{
		return (RoleNames.Any(current => current.Equals(nameof(Resources.InitialData.Roles.Admin))) || IsOwner);
	}

	public bool IsAdminCompany()
	{
		return RoleNames.Any(current => current.Equals(nameof(Resources.InitialData.Roles.AdminCompany)));
	}

	public bool IsUser()
	{
		return RoleNames.Any(current => current.Equals(nameof(Resources.InitialData.Roles.User)));
	}
}
