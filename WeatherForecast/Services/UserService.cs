using AutoMapper;
using WeatherForecast.Dto;
using WeatherForecast.Models;
using WeatherForecast.Repositories;

namespace WeatherForecast.Services;

public class UserService(IUserRepository repository, IMapper mapper) {
    public async Task<List<User>> GetAllUsersAsync() => await repository.GetAllUsersAsync();

    public async Task<UserResponseDto?> GetUserByEmailAsync(string email)
    {
        var user = await repository.GetUserByEmailAsync(email);
        return user == null ? null : mapper.Map<UserResponseDto>(user);
    }

    public async Task<UserResponseDto> CreateUserAsync(UserCreateDto userDto)
    {
        var existingUser = await repository.GetUserByEmailAsync(userDto.Email);
        if (existingUser != null)
        {
            throw new InvalidOperationException($"User with email {userDto.Email} already exists.");
        }
        var user = mapper.Map<User>(userDto);
        await repository.AddUserAsync(user);
        await repository.SaveChangesAsync();
        return mapper.Map<UserResponseDto>(user); 
    }
}