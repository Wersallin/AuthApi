using AuthApi.DTOs;
using AuthApi.Models;
using AuthApi.Repositories;

namespace AuthApi.Services;

public class AuthService(UserRepository userRepository, JwtService jwtService) : IAuthService
{
    public async Task<bool> RegisterAsync(RegisterDto dto)
    {
        bool nameExists = await userRepository.ExistsAsync(dto.Name);
        if (nameExists)
            return false;

        var user = new User
        {
            Name = dto.Name,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            RoleUser = dto.RoleUser
        };

        await userRepository.AddAsync(user);
        return true;
    }

    public async Task<string?> LoginAsync(string name, string password)
    {
        var user = await userRepository.GetByNameAsync(name);

        if (user is null)
            return null;

        bool passwordValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        if (!passwordValid)
            return null;

        return jwtService.GenerateToken(user);
    }
}
