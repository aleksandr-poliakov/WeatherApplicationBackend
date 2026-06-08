using WeatherForecast.Models;

namespace WeatherForecast.Services;

public interface ITokenService
{
    string GenerateToken(User user);
}
