using WeatherForecast.Models;

namespace WeatherForecast.Repositories;

public interface IUserRepository  {
    Task<List<User>> GetAllUsersAsync();
    Task<User?> GetUserByEmailAsync(string email);
    Task AddUserAsync(User user);
    Task SaveChangesAsync();
}