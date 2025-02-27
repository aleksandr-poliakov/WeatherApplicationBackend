using WeatherForecast.Models;

namespace WeatherForecast.Repositories;

public interface IUserRepository  {
    Task<List<User>> GetAllUsersAsync();
    Task<User?> GetUserByIdAsync(int id);
    Task AddUserAsync(User user);
    Task SaveChangesAsync();
}