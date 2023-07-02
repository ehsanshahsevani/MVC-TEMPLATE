using Domain;
using Persistence.Abstarcts;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class PostRepository : Base.Repository<Post>, IPostRepository
{
    public PostRepository(DbContext databaseContext) : base(databaseContext)
    {
    }

    public async Task<List<Post>> FindAllActivePost()
    {
        var result = await DbSet
            .Where(current => current.IsDeleted == false)
            .Where(current => current.IsActive == true)
            .ToListAsync();

        return result;
    }
}