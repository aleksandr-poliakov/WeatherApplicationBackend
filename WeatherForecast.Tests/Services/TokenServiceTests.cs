using System.IdentityModel.Tokens.Jwt;

namespace WeatherForecast.Tests.Services;

public class TokenServiceTests
{
    private readonly JwtSettings _settings = new()
    {
        Issuer = "test-issuer",
        Audience = "test-audience",
        Key = "super-secret-key-that-is-long-enough-for-hmac"
    };

    private readonly ITokenService _sut;

    public TokenServiceTests()
    {
        _sut = new TokenService(_settings);
    }

    [Fact]
    public void GenerateToken_ReturnsNonEmptyString()
    {
        var user = new User { Id = 1, Name = "Alice", Email = "alice@test.com", PasswordHash = "hash" };

        var token = _sut.GenerateToken(user);

        Assert.False(string.IsNullOrWhiteSpace(token));
    }

    [Fact]
    public void GenerateToken_ContainsEmailClaim()
    {
        var user = new User { Id = 42, Name = "Alice", Email = "alice@test.com", PasswordHash = "hash" };

        var token = _sut.GenerateToken(user);

        var parsed = new JwtSecurityTokenHandler().ReadJwtToken(token);
        var email = parsed.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?.Value;
        Assert.Equal("alice@test.com", email);
    }

    [Fact]
    public void GenerateToken_ContainsSubClaim_MatchingUserId()
    {
        var user = new User { Id = 42, Name = "Alice", Email = "alice@test.com", PasswordHash = "hash" };

        var token = _sut.GenerateToken(user);

        var parsed = new JwtSecurityTokenHandler().ReadJwtToken(token);
        var sub = parsed.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
        Assert.Equal("42", sub);
    }

    [Fact]
    public void GenerateToken_ExpiresInApproximately30Minutes()
    {
        var user = new User { Id = 1, Name = "Alice", Email = "alice@test.com", PasswordHash = "hash" };

        var before = DateTime.UtcNow.AddMinutes(29);
        var token = _sut.GenerateToken(user);
        var after = DateTime.UtcNow.AddMinutes(31);

        var parsed = new JwtSecurityTokenHandler().ReadJwtToken(token);
        Assert.True(parsed.ValidTo >= before && parsed.ValidTo <= after);
    }
}
