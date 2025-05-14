using Microsoft.AspNetCore.Identity;

namespace Presentation.Service;
public class AuthService(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager) : IAuthService
{
    private readonly UserManager<IdentityUser> _userManager = userManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;
}
