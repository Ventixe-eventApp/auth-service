using System.ComponentModel.DataAnnotations;

namespace Presentation.Models;

public class UserRegistationForm
{
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "You must enter a password")]
    public string Password { get; set; } = null!;
    [Required(ErrorMessage = "You must confirm password")]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; } = null!;

    [Range(typeof(bool), "true", "true", ErrorMessage = "You must accept terms and conditions")]
    public bool TermsAndConditions { get; set; }
}
