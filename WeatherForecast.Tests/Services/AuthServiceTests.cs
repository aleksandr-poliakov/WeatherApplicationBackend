namespace WeatherForecast.Tests.Services;

public class AuthServiceTests
{
    private readonly Mock<IUserRepository> _repository = new();
    private readonly Mock<ITokenService> _tokenService = new();
    private readonly IAuthService _sut;

    public AuthServiceTests()
    {
        _sut = new AuthService(_repository.Object, _tokenService.Object);
    }

    [Fact]
    public async Task LoginAsync_WhenUserNotFound_ReturnsNull()
    {
        _repository.Setup(r => r.GetUserByEmailAsync("nobody@test.com")).ReturnsAsync((User?)null);

        var result = await _sut.LoginAsync(new LoginRequestDto("nobody@test.com", "pass"));

        Assert.Null(result);
    }

    [Fact]
    public async Task LoginAsync_WhenPasswordIsInvalid_ReturnsNull()
    {
        var hash = BCrypt.Net.BCrypt.HashPassword("correct");
        var user = new User { Id = 1, Name = "Alice", Email = "alice@test.com", PasswordHash = hash };
        _repository.Setup(r => r.GetUserByEmailAsync("alice@test.com")).ReturnsAsync(user);

        var result = await _sut.LoginAsync(new LoginRequestDto("alice@test.com", "wrong"));

        Assert.Null(result);
        _tokenService.Verify(t => t.GenerateToken(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task LoginAsync_WhenCredentialsAreValid_ReturnsToken()
    {
        var hash = BCrypt.Net.BCrypt.HashPassword("correct");
        var user = new User { Id = 1, Name = "Alice", Email = "alice@test.com", PasswordHash = hash };
        _repository.Setup(r => r.GetUserByEmailAsync("alice@test.com")).ReturnsAsync(user);
        _tokenService.Setup(t => t.GenerateToken(user)).Returns("jwt-token");

        var result = await _sut.LoginAsync(new LoginRequestDto("alice@test.com", "correct"));

        Assert.Equal("jwt-token", result);
    }
}
