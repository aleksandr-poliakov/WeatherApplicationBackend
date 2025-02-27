using AutoMapper;
using WeatherForecast.Dto;
using WeatherForecast.Models;
using WeatherForecast.Repositories;

namespace WeatherForecast.Services;

public class UserService(IUserRepository repository, IMapper mapper) {
    public async Task<List<User>> GetAllUsersAsync() => await repository.GetAllUsersAsync();

    public async Task<UserResponseDTO?> GetUserByEmailAsync(string email)
    {
        var user = await repository.GetUserByEmailAsync(email);
        return user == null ? null : mapper.Map<UserResponseDTO>(user);
    }

    public async Task<UserResponseDTO> CreateUserAsync(UserCreateDTO userDto)
    {
        var user = mapper.Map<User>(userDto); 
        await repository.AddUserAsync(user);
        await repository.SaveChangesAsync();
        return mapper.Map<UserResponseDTO>(user); 
    }
}