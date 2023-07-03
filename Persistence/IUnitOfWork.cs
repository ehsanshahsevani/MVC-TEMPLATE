using Persistence.Abstarcts;
using Persistence.Abstracts;

namespace Persistence;

public interface IUnitOfWork : Persistence.Base.IUnitOfWork
{
	// **********
	//IXXXXXRepository XXXXXRepository { get; }
	// **********

	IPostRepository PostRepository { get; }

	public IServerLogRepository ServerLogRepository { get; }
}