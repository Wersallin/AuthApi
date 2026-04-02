using AuthApi.Data;
using AuthApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthApi.Repositories;

public class UserRepository(AppDbContext db)
{
    public async Task<bool> ExistsAsync(string name)
    {
        return await db.Users.AnyAsync(u => u.Name == name);
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await db.Users.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetByNameAsync(string name)
    {
        return await db.Users.FirstOrDefaultAsync(u => u.Name == name);
    }

    public async Task<User?> GetByRefreshTokenAsync(string refreshToken)
    {
        return await db.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
    }

    public async Task AddAsync(User user)
    {
        db.Users.Add(user);
        await db.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        db.Users.Update(user);
        await db.SaveChangesAsync();
    }
}
