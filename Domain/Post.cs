namespace Domain;

public class Post : Base.BaseEntity
{
	public Post() : base()
	{
	}

	public string? Text { get; set; }
	public string? Title { get; set; }

	public bool IsActive { get; set; }
}
