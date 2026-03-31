using AuthApi.DTOs;

namespace AuthApi.Services;

public interface IAuthService
{
    Task<bool> RegisterAsync(RegisterDto dto);
    Task<(string AccessToken, string RefreshToken)?> LoginAsync(string name, string password);
    Task<(string AccessToken, string RefreshToken)?> RefreshAsync(string refreshToken);
}
