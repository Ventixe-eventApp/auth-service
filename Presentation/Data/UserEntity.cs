using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Data;

public class UserEntity : IdentityUser
{
    [ProtectedPersonalData]
    [Required]
    [Column(TypeName = "nvarchar(100)")]
    public string FirstName { get; set; } = null!;

    [ProtectedPersonalData]
    [Required]
    [Column(TypeName = "nvarchar(100)")]
    public string LastName { get; set; } = null!;

}
