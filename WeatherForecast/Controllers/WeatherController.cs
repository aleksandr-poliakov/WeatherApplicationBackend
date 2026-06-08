using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeatherForecast.Dto;
using WeatherForecast.Services;

namespace WeatherForecast.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WeatherController(IWeatherService weatherService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<WeatherDto>> Get(
        [FromQuery] string city,
        [FromQuery] string country)
    {
        var weather = await weatherService.GetWeatherAsync(city, country);
        if (weather is null) return NotFound($"No weather found for {city}, {country}");
        return Ok(weather);
    }
}
