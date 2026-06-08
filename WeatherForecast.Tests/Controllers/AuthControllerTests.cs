namespace WeatherForecast.Tests.Controllers;

public class AuthControllerTests
{
    private readonly Mock<IAuthService> _service = new();
    private readonly AuthController _sut;

    public AuthControllerTests()
    {
        _sut = new AuthController(_service.Object);
    }

    [Fact]
    public async Task Login_WhenCredentialsValid_ReturnsOkWithToken()
    {
        var request = new LoginRequestDto("alice@test.com", "password");
        _service.Setup(s => s.LoginAsync(request)).ReturnsAsync("jwt-token");

        var result = await _sut.Login(request);

        var ok = Assert.IsType<OkObjectResult>(result);
        var json = JsonSerializer.Serialize(ok.Value);
        Assert.Contains("jwt-token", json);
    }

    [Fact]
    public async Task Login_WhenCredentialsInvalid_ReturnsUnauthorized()
    {
        var request = new LoginRequestDto("alice@test.com", "wrong");
        _service.Setup(s => s.LoginAsync(request)).ReturnsAsync((string?)null);

        var result = await _sut.Login(request);

        Assert.IsType<UnauthorizedResult>(result);
    }
}
