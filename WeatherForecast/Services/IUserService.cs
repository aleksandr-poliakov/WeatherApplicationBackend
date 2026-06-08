using WeatherForecast.Dto;

namespace WeatherForecast.Services;

public interface IUserService
{
    Task<List<UserResponseDto>> GetAllUsersAsync();
    Task<UserResponseDto?> GetUserByEmailAsync(string email);
    Task<UserResponseDto> CreateUserAsync(UserCreateDto userDto);
}
