using AuthApi.Models;

namespace AuthApi.Services;

public interface IAuthService
{
    Task<bool> RegisterAsync(RegisterRequest request);
    Task<string?> LoginAsync(string name, string password);
}
