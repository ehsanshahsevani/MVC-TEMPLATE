using Microsoft.AspNetCore.Identity;

namespace Domain;

public class User : IdentityUser<int>
{
	public User(): base()
	{
		CreateDate = DateTime.Now;
	}

	public string? FirstName { get; set; }
	public string? LastName { get; set; }

	public string? RecoveryPasswordCode { get; set; }
	public string? RecoveryPasswordToken { get; set; }

	public int Id { get; set; }
	public virtual string? Description { get; set; }

	public DateTime CreateDate { get; set; }
	public DateTime? ExpireDate { get; set; }

	public bool IsDeleted { get; set; }
}
