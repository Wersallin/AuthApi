using System.Security.Claims;
using AuthApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthApi.Controllers;

[Authorize]
[ApiController]
[Route("api/user")]
public class UserController(UserRepository userRepository) : ControllerBase
{
    [HttpGet("me")]
    public IActionResult GetMe()
    {
        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var name = User.FindFirst(ClaimTypes.Name)?.Value;
        var role = User.FindFirst(ClaimTypes.Role)?.Value;

        return Ok(new { id, name, role });
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        var users = await userRepository.GetAllAsync();

        var result = users.Select(u => new
        {
            u.Id,
            u.Name,
            u.RoleUser
        });

        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var adminIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        int adminId = int.Parse(adminIdClaim!);

        if (adminId == id)
            return BadRequest(new { message = "Voce nao pode deletar sua propria conta." });

        bool success = await userRepository.DeleteAsync(id);

        if (!success)
            return NotFound(new { message = "Usuario nao encontrado." });

        return Ok(new { message = "Usuario deletado com sucesso." });
    }
}
