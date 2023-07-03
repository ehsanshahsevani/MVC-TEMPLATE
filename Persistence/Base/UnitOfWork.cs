using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Persistence.Base;

public abstract class UnitOfWork : IUnitOfWork
{
	public UnitOfWork(Tools.Options options) : base()
	{
		Options = options;
	}

	// **********
	protected Tools.Options Options { get; set; }
	// **********

	// **********
	// **********
	// **********
	private DatabaseContext? _databaseContext;
	// **********

	// **********
	/// <summary>
	/// Lazy Loading = Lazy Initialization
	/// </summary>
	internal DatabaseContext DatabaseContext
	{
		get
		{
			if (_databaseContext == null)
			{
				var optionsBuilder =
					new DbContextOptionsBuilder<DatabaseContext>();

				switch (Options.Provider)
				{
					case Tools.Enums.Provider.SqlServer:
						{
							optionsBuilder.UseSqlServer
								(connectionString: Options.ConnectionString);

							break;
						}

					case Tools.Enums.Provider.MySql:
						{
							//optionsBuilder.UseMySql
							//	(connectionString: Options.ConnectionString);

							break;
						}

					case Tools.Enums.Provider.Oracle:
						{
							//optionsBuilder.UseOracle
							//	(connectionString: Options.ConnectionString);

							break;
						}

					case Tools.Enums.Provider.PostgreSQL:
						{
							//optionsBuilder.UsePostgreSQL
							//	(connectionString: Options.ConnectionString);

							break;
						}

					case Tools.Enums.Provider.InMemory:
						{
							// optionsBuilder.UseInMemoryDatabase(databaseName: "Temp");

							break;
						}
				}

				_databaseContext =
					new DatabaseContext(options: optionsBuilder.Options);
			}

			return _databaseContext;
		}
	}
	// **********
	/// <summary>
	/// To detect redundant calls
	/// </summary>
	public bool IsDisposed { get; protected set; }
	// **********

	private IDbContextTransaction? Transaction { get; set; }

	// **********
	public async Task<IDbContextTransaction> BeginTransactionAsync()
	{
		Transaction ??=
			await DatabaseContext.Database.BeginTransactionAsync();

		return Transaction;
	}
	// **********

	// **********
	public async Task MigrateAsync()
	{
		await DatabaseContext.Database.MigrateAsync();
	}
	// **********


	/// <summary>
	/// Public implementation of Dispose pattern callable by consumers.
	/// </summary>
	public void Dispose()
	{
		Dispose(true);

		GC.SuppressFinalize(this);
	}

	/// <summary>
	/// https://docs.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose
	/// </summary>
	protected virtual void Dispose(bool disposing)
	{
		if (IsDisposed)
		{
			return;
		}

		if (disposing)
		{
			// TODO: dispose managed state (managed objects).

			if (DatabaseContext != null)
			{
				DatabaseContext.Dispose();
			}
		}

		// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
		// TODO: set large fields to null.

		IsDisposed = true;
	}

	public async Task<int> SaveAsync(DetailsLog detailsLog)
	{
		int result = 0;

		var detailsLogs = new List<DetailsLog>();

		var Entries = DatabaseContext.ChangeTracker.Entries();

		JsonSerializerOptions jsonSerializerOptions =
			new JsonSerializerOptions()
			{
				ReferenceHandler = ReferenceHandler.IgnoreCycles,
			};

		foreach (var item in Entries)
		{
			if (item.State == EntityState.Unchanged)
			{
				continue;
			}

			detailsLog.EntityTracker = item.Entity;

			detailsLog.DebugView = item.DebugView.LongView;

			detailsLog.StateChangeWorker = item.State.ToString();

			detailsLog.NameSpace = item.Entity.ToString();
			detailsLog.TypeName = item.Entity.GetType().Name;

			if (string.IsNullOrEmpty(detailsLog.TypeName) == false
				&& detailsLog.TypeName != $"{nameof(ServerLog)}"
				&& detailsLog.TypeName != $"{nameof(DetailsLog)}")
			{
				detailsLogs.Add(detailsLog.Clone<DetailsLog>());
			}
		}

		result =
			await DatabaseContext.SaveChangesAsync();

		if (detailsLogs.Count != 0)
		{
			detailsLogs.ForEach(item =>
			{
				item.JsonField =
					JsonSerializer.Serialize(item.EntityTracker, jsonSerializerOptions);

				item.RecordId = (item.EntityTracker as Domain.Base.BaseEntity)!.Id;
			});

			await DatabaseContext.DetailsLogs.AddRangeAsync(detailsLogs);
			await DatabaseContext.SaveChangesAsync();
		}

		return result;
	}

	public async Task<int> SendToDatabaseAsync()
	{
		int result =
			await DatabaseContext.SaveChangesAsync();

		return result;
	}

	~UnitOfWork()
	{
		Dispose(false);
	}
}
