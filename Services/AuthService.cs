using AuthApi.Data;
using AuthApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthApi.Services;

public class AuthService(AppDbContext db, JwtService jwtService) : IAuthService
{
    public async Task<bool> RegisterAsync(RegisterRequest request)
    {
        bool nameExists = await db.Users.AnyAsync(u => u.Name == request.Name);
        if (nameExists)
            return false;

        var user = new User
        {
            Name = request.Name,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            RoleUser = request.RoleUser
        };

        db.Users.Add(user);
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<string?> LoginAsync(string name, string password)
    {
        var user = await db.Users.FirstOrDefaultAsync(u => u.Name == name);

        if (user is null)
            return null;

        bool passwordValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        if (!passwordValid)
            return null;

        return jwtService.GenerateToken(user);
    }
}
