using Microsoft.AspNetCore.Mvc;
using WeatherForecast.Dto;
using WeatherForecast.Services;

namespace WeatherForecast.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginRequestDto request)
    {
        var token = await authService.LoginAsync(request);
        if (token is null) return Unauthorized();
        return Ok(new { token });
    }
}
