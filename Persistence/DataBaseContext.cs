using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

internal class DatabaseContext : IdentityDbContext<Domain.User, Domain.Role, int> // DbContext
{
	//public DatabaseContext()
	//{
	//}

#pragma warning disable CS8618
	public DatabaseContext
#pragma warning restore CS8618
		(DbContextOptions<DatabaseContext> options)
		: base(options)
	{
	}

	#region All DbSets
	public DbSet<Domain.Post> Posts { get; set; }
	public DbSet<Domain.ClientLog> ClientLogs { get; set; }
	public DbSet<Domain.DetailsLog> DetailsLogs { get; set; }
	public DbSet<Domain.ServerLog> ServerLogs { get; set; }
	#endregion

	#region OnConfiguring
	//protected override void OnConfiguring(DbDatabaseContextOptionsBuilder optionsBuilder)
	//{
	//	optionsBuilder
	//		.UseSqlServer("Data Source=.;initial catalog=BlogDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
	//	base.OnConfiguring(optionsBuilder);
	//}
	#endregion

	protected override void OnModelCreating(ModelBuilder builder)
	{
		// settings relations

		base.OnModelCreating(builder);
	}
}
