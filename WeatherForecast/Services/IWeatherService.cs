using WeatherForecast.Dto;

namespace WeatherForecast.Services;

public interface IWeatherService
{
    Task<WeatherDto?> GetWeatherAsync(string city, string country);
}
