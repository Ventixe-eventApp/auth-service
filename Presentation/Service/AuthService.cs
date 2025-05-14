using Microsoft.AspNetCore.Identity;
using Presentation.Data;
using Presentation.Models;

namespace Presentation.Service;
public class AuthService(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, DataContext context) : IAuthService
{
    private readonly UserManager<IdentityUser> _userManager = userManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;
    private readonly DataContext _context = context;

    public async Task<(bool Success, string ErrorMessage)> RegisterUserAsync(UserRegistationForm form)
    {
        ArgumentNullException.ThrowIfNull(form);

        var userExists = await _userManager.FindByEmailAsync(form.Email);
        if (userExists != null)
        {
            return (false, "User with same email already exist");
        }



    }
}
