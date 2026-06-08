using WeatherForecast.Tests.Helpers;

namespace WeatherForecast.Tests.Services;

public class WeatherServiceTests
{
    private readonly WeatherApiSettings _settings = new()
    {
        GeocodingUrl = "https://geocoding-api.test/v1/search",
        ForecastUrl = "https://api.weather.test/v1/forecast"
    };

    private (IWeatherService sut, FakeHttpMessageHandler handler) Build()
    {
        var handler = new FakeHttpMessageHandler();
        var client = new HttpClient(handler);
        return (new WeatherService(client, _settings), handler);
    }

    private static object GeocodingPayload(params (string Name, string Country, double Lat, double Lon)[] results) =>
        new
        {
            results = results.Select(r => new
            {
                name = r.Name,
                country = r.Country,
                latitude = r.Lat,
                longitude = r.Lon
            }).ToArray()
        };

    private static object ForecastPayload(double temp, int code, double wind) =>
        new
        {
            current = new
            {
                temperature_2m = temp,
                weather_code = code,
                wind_speed_10m = wind
            }
        };

    [Fact]
    public async Task GetWeatherAsync_WhenGeocodingReturnsNullResults_ReturnsNull()
    {
        var (sut, handler) = Build();
        handler.Enqueue(new { results = (object?)null });

        var result = await sut.GetWeatherAsync("Unknown", "NoWhere");

        Assert.Null(result);
    }

    [Fact]
    public async Task GetWeatherAsync_WhenGeocodingReturnsEmptyResults_ReturnsNull()
    {
        var (sut, handler) = Build();
        handler.Enqueue(new { results = Array.Empty<object>() });

        var result = await sut.GetWeatherAsync("Unknown", "NoWhere");

        Assert.Null(result);
    }

    [Fact]
    public async Task GetWeatherAsync_WhenCityFound_ReturnsMappedWeatherDto()
    {
        var (sut, handler) = Build();
        handler.Enqueue(GeocodingPayload(("London", "United Kingdom", 51.5074, -0.1278)));
        handler.Enqueue(ForecastPayload(15.5, 1, 10.2));

        var result = await sut.GetWeatherAsync("London", "United Kingdom");

        Assert.NotNull(result);
        Assert.Equal("London", result.City);
        Assert.Equal("United Kingdom", result.Country);
        Assert.Equal(15.5, result.TemperatureC);
        Assert.Equal(10.2, result.WindSpeedKmh);
        Assert.Equal("Partly cloudy", result.Condition);
    }

    [Fact]
    public async Task GetWeatherAsync_WhenExactCountryMatch_PrefersMatchingCity()
    {
        var (sut, handler) = Build();
        handler.Enqueue(GeocodingPayload(
            ("London", "Canada", 42.98, -81.24),
            ("London", "United Kingdom", 51.5074, -0.1278)));
        handler.Enqueue(ForecastPayload(8.0, 61, 5.0));

        var result = await sut.GetWeatherAsync("London", "United Kingdom");

        Assert.NotNull(result);
        Assert.Equal("United Kingdom", result.Country);
    }

    [Fact]
    public async Task GetWeatherAsync_WhenNoExactCountryMatch_FallsBackToFirstResult()
    {
        var (sut, handler) = Build();
        handler.Enqueue(GeocodingPayload(("London", "Canada", 42.98, -81.24)));
        handler.Enqueue(ForecastPayload(5.0, 71, 3.0));

        var result = await sut.GetWeatherAsync("London", "United Kingdom");

        Assert.NotNull(result);
        Assert.Equal("Canada", result.Country);
    }

    [Theory]
    [InlineData(0, "Clear")]
    [InlineData(1, "Partly cloudy")]
    [InlineData(2, "Partly cloudy")]
    [InlineData(3, "Partly cloudy")]
    [InlineData(45, "Fog")]
    [InlineData(48, "Fog")]
    [InlineData(51, "Drizzle")]
    [InlineData(61, "Rain")]
    [InlineData(71, "Snow")]
    [InlineData(80, "Rain showers")]
    [InlineData(95, "Thunderstorm")]
    [InlineData(999, "Unknown")]
    public async Task GetWeatherAsync_MapsWeatherCode(int code, string expectedCondition)
    {
        var (sut, handler) = Build();
        handler.Enqueue(GeocodingPayload(("City", "Country", 0.0, 0.0)));
        handler.Enqueue(ForecastPayload(20.0, code, 5.0));

        var result = await sut.GetWeatherAsync("City", "Country");

        Assert.NotNull(result);
        Assert.Equal(expectedCondition, result.Condition);
    }
}
