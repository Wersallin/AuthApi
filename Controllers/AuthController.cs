using AuthApi.Models;
using AuthApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace AuthApi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        bool success = await authService.RegisterAsync(request);

        if (!success)
            return Conflict(new { message = "Nome de usuario ja esta em uso." });

        return Ok(new { message = "Usuario criado com sucesso." });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        string? token = await authService.LoginAsync(request.Name, request.Password);

        if (token is null)
            return Unauthorized(new { message = "Nome ou senha invalidos." });

        return Ok(new { token });
    }
}
