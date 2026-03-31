using AuthApi.Data;
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

    public async Task<(string AccessToken, string RefreshToken)?> LoginAsync(string name, string password)
    {
        var user = await userRepository.GetByNameAsync(name);

        if (user is null)
            return null;

        bool passwordValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        if (!passwordValid)
            return null;

        var accessToken = jwtService.GenerateToken(user);
        var refreshToken = jwtService.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        await userRepository.UpdateAsync(user);

        return (accessToken, refreshToken);
    }

    public async Task<(string AccessToken, string RefreshToken)?> RefreshAsync(string refreshToken)
    {
        var user = await userRepository.GetByRefreshTokenAsync(refreshToken);

        if (user is null)
            return null;

        if (user.RefreshTokenExpiry < DateTime.UtcNow)
            return null;

        var newAccessToken = jwtService.GenerateToken(user);
        var newRefreshToken = jwtService.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        await userRepository.UpdateAsync(user);

        return (newAccessToken, newRefreshToken);
    }
}
