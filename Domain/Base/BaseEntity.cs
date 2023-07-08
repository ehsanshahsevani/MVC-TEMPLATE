

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

	public int Id { get; private set; }
	// **********

	// **********
	[System.ComponentModel.DataAnnotations.MaxLength(150)]

	public virtual string? Description { get; set; }
	// **********

	public DateTime InsertDateTime { get; private set; }

	public bool IsDeleted { get; set; }
}
