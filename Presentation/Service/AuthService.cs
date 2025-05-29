using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using Presentation.Models;
using System.Net.Http.Json;

namespace Presentation.Service;
public class AuthService : IAuthService
{
    public async Task<bool> AlreadyExistAsync(string email)
    {
        using var http = new HttpClient();
        var response = await http.GetFromJsonAsync<AccountResult>($"https://localhost:7150/api/accounts/exists?email={email}");

        if (!response.Succeeded)
            return false;

        return true;
    }


    public async Task<AuthResult<string>> RegisterUserAsync(UserRegistationForm form)
    {
        var exists = await AlreadyExistAsync(form.Email);
        if (exists)
        {
            return new AuthResult<string>
            {
                Succeeded = false,
                StatusCode = 400,
                Error = "Email already exists"
            };
        }

        using var http = new HttpClient();

        var registerRequest = new RegisterAccountRequest
        {
            Email = form.Email,
            Password = form.Password
        };

        var accountResponse = await http.PostAsJsonAsync("https://localhost:7150/api/accounts/register", registerRequest);

        if (!accountResponse.IsSuccessStatusCode)
        {
            var error = await accountResponse.Content.ReadAsStringAsync();
            return new AuthResult<string>
            {
                Succeeded = false,
                StatusCode = (int)accountResponse.StatusCode,
                Error = $"AccountService error: {error}"
            };
        }

        var accountData = await accountResponse.Content.ReadFromJsonAsync<RegisterAccountResponse>();

        if (accountData == null || string.IsNullOrEmpty(accountData.UserId))
        {
            return new AuthResult<string>
            {
                Succeeded = false,
                StatusCode = 500,
                Error = "Invalid response from AccountService"
            };
        }

        return new AuthResult<string>
        {
            Succeeded = true,
            StatusCode = 200,
            Result = accountData.UserId
        };
    }

    public async Task<AuthResult> RegisterUserProfileAsync(UserProfileRequest request)
    {

        using var http = new HttpClient();
        var response = await http.PostAsJsonAsync("https://localhost:7026/api/Users/create", request);
        if (response.IsSuccessStatusCode)
        {
            return new AuthResult
            {
                Succeeded = true,
                StatusCode = 200
            };
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            return new AuthResult
            {
                Succeeded = false,
                StatusCode = (int)response.StatusCode,
                Error = $"Profile creation failed: {error}"
            };
        }
    }


    public async Task<AuthResult<LoginResponse>> LoginAsync(LoginRequest request)
    {
        using var http = new HttpClient();

        var response = await http.PostAsJsonAsync("https://localhost:7150/api/accounts/login", request);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            return new AuthResult<LoginResponse>
            {
                Succeeded = false,
                StatusCode = (int)response.StatusCode,
                Error = $"Login failed: {error}"
            };
        }


        var loginResponse = await response.Content.ReadFromJsonAsync<AccountResult<LoginWithAccountResponse>>();

        if (loginResponse == null || loginResponse.Result == null )
        {
            return new AuthResult<LoginResponse>
            {
                Succeeded = false,
                StatusCode = 400,
                Error = "Invalid login response from AccountService"

            };
        }
        
        var userId = loginResponse.Result.UserId;

        var userResponse = await http.GetAsync($"https://localhost:7026/api/users/{userId}");

        if (!userResponse.IsSuccessStatusCode)
        {
            var error = await userResponse.Content.ReadAsStringAsync();
            return new AuthResult<LoginResponse>
            {
                Succeeded = false,
                StatusCode = (int)userResponse.StatusCode,
                Error = $"Failed to retrieve user profile: {error}"
            };
        }

        var userResult = await userResponse.Content.ReadFromJsonAsync<UserResult<User>>();

        if (userResult == null || userResult.Result == null)
        {
            return new AuthResult<LoginResponse>
            {
                Succeeded = false,
                StatusCode = 404,
                Error = "No user profile found"
            };
        }

        var user = userResult.Result;


        return new AuthResult<LoginResponse>
        {
            Succeeded = true,
            StatusCode = 200,
            Result = new LoginResponse
            {
                UserId = user.UserId,
                FirstName = user.FirstName!,
                LastName = user.LastName!,
                Email = request.Email
            }
        };
    }

    public async Task<AuthResult> LogoutAsync()
    {
        using var http = new HttpClient();

        var response = await http.PostAsync("https://localhost:7150/api/accounts/logout", null);
        if (response.IsSuccessStatusCode)
        {
            return new AuthResult
            {
                Succeeded = true,
                StatusCode = 200
            };
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            return new AuthResult
            {
                Succeeded = false,
                StatusCode = (int)response.StatusCode,
                Error = $"Logout failed: {error}"
            };
        }
    }
}
