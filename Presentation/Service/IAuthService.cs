using Presentation.Models;

namespace Presentation.Service;

public interface IAuthService
{

    Task<bool> AlreadyExistAsync(string email);
    Task<AuthResult> LoginAsync(LoginRequest request);
    Task<AuthResult> RegisterUserProfileAsync(UserProfileRequest request);
    Task<AuthResult<string>> RegisterUserAsync(UserRegistationForm form);
}
