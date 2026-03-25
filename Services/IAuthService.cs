using AuthApi.DTOs;

namespace AuthApi.Services;

public interface IAuthService
{
    Task<bool> RegisterAsync(RegisterDto dto);
    Task<string?> LoginAsync(string name, string password);
}
