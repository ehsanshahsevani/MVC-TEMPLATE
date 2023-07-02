namespace Persistence.Abstarcts;

public interface IPostRepository : Base.IRepository<Domain.Post>
{
	Task<List<Domain.Post>> FindAllActivePost();
}
