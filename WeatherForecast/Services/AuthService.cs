using WeatherForecast.Dto;
using WeatherForecast.Repositories;

namespace WeatherForecast.Services;

public class AuthService(IUserRepository repository, ITokenService tokenService) : IAuthService
{
    public async Task<string?> LoginAsync(LoginRequestDto request)
    {
        var user = await repository.GetUserByEmailAsync(request.Email);
        if (user is null) return null;

        var valid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
        if (!valid) return null;

        return tokenService.GenerateToken(user);
    }
}
