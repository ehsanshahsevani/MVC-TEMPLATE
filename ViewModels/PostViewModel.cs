using ViewModels.Base;

namespace Domain;

public class PostViewModel : BaseViewModel
{
	public PostViewModel() : base()
	{
	}

	public string? Text { get; set; }
	public string? Title { get; set; }

	public bool IsActive { get; set; }
}
