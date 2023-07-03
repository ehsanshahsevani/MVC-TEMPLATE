using Persistence.Abstarcts;
using Persistence.Abstracts;
using Persistence.Repositories;

namespace Persistence;

public class UnitOfWork : Base.UnitOfWork, IUnitOfWork
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	public UnitOfWork(Tools.Options options) : base(options)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	{
	}

	// **************************************************
	//private IXXXXXRepository _xXXXXRepository;

	//public IXXXXXRepository XXXXXRepository
	//{
	//	get
	//	{
	//		if (_xXXXXRepository == null)
	//		{
	//			_xXXXXRepository =
	//				new XXXXXRepository(DatabaseContext);
	//		}

	//		return _xXXXXRepository;
	//	}
	//}
	// **************************************************

	// **************************************************
	private IPostRepository _PostRepository;

	public IPostRepository PostRepository
	{
		get
		{
			if (_PostRepository == null)
			{
				_PostRepository =
					new PostRepository(DatabaseContext);
			}

			return _PostRepository;
		}
	}
	// **************************************************

	// **************************************************
	private IServerLogRepository _ServerLogRepository;

	public IServerLogRepository ServerLogRepository
	{
		get
		{
			if (_ServerLogRepository == null)
			{
				_ServerLogRepository =
					new ServerLogRepository(DatabaseContext);
			}

			return _ServerLogRepository;
		}
	}
	// **************************************************
}