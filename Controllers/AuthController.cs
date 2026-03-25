using AuthApi.DTOs;
using AuthApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace AuthApi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        bool success = await authService.RegisterAsync(dto);

        if (!success)
            return Conflict(new { message = "Nome de usuario ja esta em uso." });

        return Ok(new { message = "Usuario criado com sucesso." });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        string? token = await authService.LoginAsync(dto.Name, dto.Password);

        if (token is null)
            return Unauthorized(new { message = "Nome ou senha invalidos." });

        return Ok(new { token });
    }
}
