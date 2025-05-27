using Presentation.Models;

namespace Presentation.Service;

public interface IAuthService
{
    Task<AuthResult> RegisterUserAsync(UserRegistationForm form);

    Task<bool> AlreadyExistAsync(string email);
    Task<AuthResult> LoginAsync(LoginRequest request);
    Task<AuthResult> RegisterUserProfileAsync(UserProfileRequest request);
}
