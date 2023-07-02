using Domain;
using Microsoft.EntityFrameworkCore.Storage;

namespace Persistence.Base;

public interface IUnitOfWork : IDisposable
{
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
	void Dispose();
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
	Task<int> SendToDatabaseAsync();
	Task<int> SaveAsync(DetailsLog detailsLog);
	// **********
	Task<IDbContextTransaction> BeginTransactionAsync();

	Task MigrateAsync();
	// **********
}