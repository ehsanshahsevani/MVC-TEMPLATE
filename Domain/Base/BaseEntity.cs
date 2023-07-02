namespace Domain.Base;

public abstract class BaseEntity : object
{
	public BaseEntity()
	{
		CreateDate = DateTime.Now;
	}

	public int Id { get; set; }
	public virtual string? Description { get; set; }

	public DateTime CreateDate { get; set; }
	public DateTime? ExpireDate { get; set; }

	public bool IsDeleted { get; set; }
}
