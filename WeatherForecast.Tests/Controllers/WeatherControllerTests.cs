namespace WeatherForecast.Tests.Controllers;

public class WeatherControllerTests
{
    private readonly Mock<IWeatherService> _service = new();
    private readonly WeatherController _sut;

    public WeatherControllerTests()
    {
        _sut = new WeatherController(_service.Object);
    }

    [Fact]
    public async Task Get_WhenWeatherFound_ReturnsOkWithDto()
    {
        var dto = new WeatherDto("London", "United Kingdom", 15.5, 10.2, "Partly cloudy");
        _service.Setup(s => s.GetWeatherAsync("London", "United Kingdom")).ReturnsAsync(dto);

        var result = await _sut.Get("London", "United Kingdom");

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var body = Assert.IsType<WeatherDto>(ok.Value);
        Assert.Equal("London", body.City);
        Assert.Equal("Partly cloudy", body.Condition);
    }

    [Fact]
    public async Task Get_WhenWeatherNotFound_ReturnsNotFound()
    {
        _service.Setup(s => s.GetWeatherAsync("Unknown", "NoWhere")).ReturnsAsync((WeatherDto?)null);

        var result = await _sut.Get("Unknown", "NoWhere");

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }
}
