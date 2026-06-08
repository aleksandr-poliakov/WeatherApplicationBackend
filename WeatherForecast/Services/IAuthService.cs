using WeatherForecast.Dto;

namespace WeatherForecast.Services;

public interface IAuthService
{
    Task<string?> LoginAsync(LoginRequestDto request);
}
