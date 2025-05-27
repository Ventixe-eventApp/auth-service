using Presentation.Models;

namespace Presentation.Service;

public interface IAuthService
{
    Task<AuthResult> RegisterUserAsync(UserRegistationForm form);

    Task<bool> AlreadyExistAsync(string email);

}
