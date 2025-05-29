using Presentation.Models;

namespace Presentation.Service;

public interface IAuthService
{

    Task<bool> AlreadyExistAsync(string email);
    Task<AuthResult> RegisterUserProfileAsync(UserProfileRequest request);
    Task<AuthResult<string>> RegisterUserAsync(UserRegistationForm form);
    Task<AuthResult<LoginResponse>> LoginAsync(LoginRequest request);
    Task<AuthResult> LogoutAsync();
}
