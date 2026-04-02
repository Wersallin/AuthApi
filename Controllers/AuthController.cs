using System.Security.Claims;
using AuthApi.DTOs;
using AuthApi.Services;
using Microsoft.AspNetCore.Authorization;
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

        var result = await authService.LoginAsync(dto.Name, dto.Password);

        if (result is null)
            return Unauthorized(new { message = "Nome ou senha invalidos." });

        return Ok(new
        {
            accessToken = result.Value.AccessToken,
            refreshToken = result.Value.RefreshToken
        });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await authService.RefreshAsync(dto.RefreshToken);

        if (result is null)
            return Unauthorized(new { message = "Refresh token invalido ou expirado." });

        return Ok(new
        {
            accessToken = result.Value.AccessToken,
            refreshToken = result.Value.RefreshToken
        });
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userIdClaim is null)
            return Unauthorized(new { message = "Usuario nao autenticado." });

        int userId = int.Parse(userIdClaim);
        bool success = await authService.LogoutAsync(userId);

        if (!success)
            return NotFound(new { message = "Usuario nao encontrado." });

        return Ok(new { message = "Logout realizado com sucesso." });
    }
}
