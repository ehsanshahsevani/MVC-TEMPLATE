using Microsoft.AspNetCore.Identity;

namespace Domain;

public class Role : IdentityRole<int>
{
    public Role() : base()
    {
    }

	public string? Title { get; set; }
}
