using System.ComponentModel.DataAnnotations;

namespace Presentation.Models;

public class UserRegistationForm
{
    [Required(ErrorMessage = "You must enter a firstname")]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "You must enter a lastname")]
    public string LastName { get; set; } = null!;
    [Required(ErrorMessage = "You must enter an email")]
    public string Email { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    [Required(ErrorMessage = "You must enter a password")]
    public string Password { get; set; } = null!;
    [Required(ErrorMessage = "You must confirm password")]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; } = null!;

    [Range(typeof(bool), "true", "true", ErrorMessage = "You must accept terms and conditions")]
    public bool TermsAndConditions { get; set; }
}
