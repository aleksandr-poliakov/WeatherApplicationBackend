using WeatherForecast.Dto;
using WeatherForecast.Models;
using WeatherForecast.Repositories;

namespace WeatherForecast.Services;

public class UserService(IUserRepository repository) {
    public async Task<List<User>> GetAllUsersAsync() => await repository.GetAllUsersAsync();

    public async Task<User?> GetUserByIdAsync(int id) => await repository.GetUserByIdAsync(id);

    public async Task<User> CreateUserAsync(UserCreateDTO userDto)
    {
        var user = new User(userDto.Name, userDto.Email);
        await repository.AddUserAsync(user);
        await repository.SaveChangesAsync();
        return user; 
    }
}