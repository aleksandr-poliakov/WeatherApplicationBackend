using System.Text.Json.Serialization;

namespace WeatherForecast.Integrations;

public record GeocodingResult(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("country")] string Country,
    [property: JsonPropertyName("latitude")] double Latitude,
    [property: JsonPropertyName("longitude")] double Longitude);