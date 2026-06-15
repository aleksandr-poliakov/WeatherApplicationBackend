using WeatherForecast.Exception;

namespace WeatherForecast.Controllers;

using Microsoft.AspNetCore.Mvc;
using WeatherForecast.Dto;
using WeatherForecast.Services;


[ApiController]
[Route("api/[controller]")]
public class SubscriptionController(IScheduleGeneratorService generator) : ControllerBase
{
    [HttpPost("generate")]
    public IActionResult Generate([FromBody] CreateSubscriptionDto dto)
    {
        try
        {
            var schedule = generator.Generate(dto);
            return Ok(schedule);
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (System.Exception ex)
        {
            return StatusCode(500, new { error = "An unexpected error occurred.", detail = ex.Message });
        }
    }
}