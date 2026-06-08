using System.Text.Json.Serialization;

namespace WeatherForecast.Integrations;

public record ForecastResponse(
    [property: JsonPropertyName("current")] CurrentWeather Current);