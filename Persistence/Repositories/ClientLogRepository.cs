using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence.Abstracts;

namespace Persistence.Repositories;

internal class ClientLogRepository : Persistence.Repository<ClientLog>, IClientLogRepository
{
    internal ClientLogRepository(DbContext databaseContext) : base(databaseContext)
    {
    }
}
