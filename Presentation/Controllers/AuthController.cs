using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;
using Presentation.Service;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUserAsync([FromBody] UserRegistationForm form)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
                );

            return BadRequest(new { success = false, errors });
        }

        var result = await _authService.RegisterUserAsync(form);

        if (result.Succeeded)
        {
            return Ok(new
            {
                success = true,
                message = "User created successfully",
                userId = result.Result 
            });
        }

        return BadRequest(new
        {
            success = false,
            message = result.Error
        });
    }

   
    [HttpPost("createprofile")]
    public async Task<IActionResult> RegisterUserProfileAsync([FromBody] UserProfileRequest request)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
                );
            return BadRequest(new { sucess = false, errors });
        }

        var result = await _authService.RegisterUserProfileAsync(request);
        if (result.Succeeded)
        {
            return Ok(new { message = "Userprofile created successfully" });
        }
        else
        {

            return BadRequest(new { message = result.Error });
        }
    }

    [HttpPost("login")] 
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
                );
            return BadRequest(new { sucess = false, errors });
        }
        var result = await _authService.LoginAsync(request);

        if (result.Succeeded)
        {
            return Ok(new { message = "Login successful"});
        }
        else
        {
            return BadRequest(new { message = result.Error });
        }
    }



}
