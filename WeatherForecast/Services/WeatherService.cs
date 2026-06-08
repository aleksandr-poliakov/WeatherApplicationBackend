using WeatherForecast.Dto;
using WeatherForecast.Integrations;
using WeatherForecast.Settings;

namespace WeatherForecast.Services;

public class WeatherService(HttpClient http, WeatherApiSettings settings) : IWeatherService
{
    public async Task<WeatherDto?> GetWeatherAsync(string city, string country)
    {
        var geo = await http.GetFromJsonAsync<GeocodingResponse>(
            $"{settings.GeocodingUrl}?name={Uri.EscapeDataString(city)}&count=10&language=en&format=json");

        var place = geo?.Results?.FirstOrDefault(r =>
                        string.Equals(r.Country, country, StringComparison.OrdinalIgnoreCase))
                    ?? geo?.Results?.FirstOrDefault();

        if (place is null) return null;

        var forecastUrl = FormattableString.Invariant(
            $"{settings.ForecastUrl}?latitude={place.Latitude}&longitude={place.Longitude}&current=temperature_2m,weather_code,wind_speed_10m");

        var forecast = await http.GetFromJsonAsync<ForecastResponse>(forecastUrl);
        if (forecast is null) return null;

        return new WeatherDto(
            place.Name,
            place.Country,
            forecast.Current.Temperature,
            forecast.Current.WindSpeed,
            MapCondition(forecast.Current.WeatherCode));
    }

    private static string MapCondition(int code) => code switch
    {
        0 => "Clear",
        1 or 2 or 3 => "Partly cloudy",
        45 or 48 => "Fog",
        51 or 53 or 55 => "Drizzle",
        61 or 63 or 65 => "Rain",
        71 or 73 or 75 => "Snow",
        80 or 81 or 82 => "Rain showers",
        95 or 96 or 99 => "Thunderstorm",
        _ => "Unknown"
    };
}
