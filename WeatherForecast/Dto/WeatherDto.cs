namespace WeatherForecast.Dto;

public record WeatherDto(
    string City,
    string Country,
    double TemperatureC,
    double WindSpeedKmh,
    string Condition);