

namespace Domain.Base;

public abstract class BaseEntity : object
{
	public BaseEntity()
	{
		InsertDateTime = DateTime.Now;
	}

	// **********
	[System.ComponentModel.DataAnnotations.Schema.DatabaseGenerated
		(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]

	[System.ComponentModel.DataAnnotations.Key]

	public int Id { get; set; }
	// **********

	public virtual string? Description { get; set; }

	public DateTime InsertDateTime { get; set; }

	public bool IsDeleted { get; set; }
}
