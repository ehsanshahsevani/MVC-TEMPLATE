using System.ComponentModel.DataAnnotations;

namespace ViewModels;

public class LoginViewModel: Base.BaseViewModel
{
    public LoginViewModel() : base()
    {
    }

    public string? UserName { get; set; }

    public string? Password { get; set; }

    // g-recaptcha-response
    public string? ResponseCaptcha { get; set; }

    public string? Token { get; set; }

    public string? NewPassword { get; set; }
    public string? OldPassword { get; set; }
}
