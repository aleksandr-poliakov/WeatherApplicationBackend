using Microsoft.EntityFrameworkCore;
using WeatherForecast.Data;
using WeatherForecast.Models;

namespace WeatherForecast.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
    public async Task<List<User>> GetAllUsersAsync() => await context.Users.ToListAsync();

    public async Task<User?> GetUserByEmailAsync(string email) =>
        await context.Users.FirstOrDefaultAsync(u => u.Email == email);

    public async Task AddUserAsync(User user)
    {
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
    }
}
