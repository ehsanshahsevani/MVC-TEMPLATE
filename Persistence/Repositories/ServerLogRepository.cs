using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence.Abstracts;

namespace Persistence.Repositories;

internal class ServerLogRepository : Persistence.Repository<ServerLog>, IServerLogRepository
{
	internal ServerLogRepository(DbContext databaseContext) : base(databaseContext)
	{
	}
}
