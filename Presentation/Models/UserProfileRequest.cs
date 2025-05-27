namespace Presentation.Models;

public class UserProfileRequest
{

    public string UserId { get; set; } = null!; 
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string? StreetName { get; set; } 
    public string? PostalCode { get; set; } 
    public string? City { get; set; }
    public string? Country { get; set; } 

}
