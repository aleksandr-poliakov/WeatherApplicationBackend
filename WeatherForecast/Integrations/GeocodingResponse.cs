using System.Text.Json.Serialization;

namespace WeatherForecast.Integrations;

public record GeocodingResponse(
    [property: JsonPropertyName("results")] List<GeocodingResult>? Results);