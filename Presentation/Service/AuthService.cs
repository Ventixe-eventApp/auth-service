using Microsoft.AspNetCore.Identity;
using Presentation.Models;
using System.Net.Http.Json;

namespace Presentation.Service;
public class AuthService : IAuthService
{
    public async Task<bool> AlreadyExistAsync(string email)
    {
        using var http = new HttpClient();
        var response = await http.GetFromJsonAsync<AccountResult>($"https://localhost:7150/api/accounts/exists?email={email}");

        return response.Succeeded;
    }

    public async Task<AuthResult> RegisterUserAsync(UserRegistationForm form)
    {
        var exists = await AlreadyExistAsync(form.Email);
        if (exists)
        {
            return new AuthResult
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
}
