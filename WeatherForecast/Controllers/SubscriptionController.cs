using Microsoft.AspNetCore.Mvc;
using WeatherForecast.Dto;
using WeatherForecast.Exception;
using WeatherForecast.Services;

namespace WeatherForecast.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubscriptionController(ISubscriptionService subscriptionService) : ControllerBase
{
    [HttpPost("generate")]
    public async Task<IActionResult> Generate([FromBody] CreateSubscriptionDto dto)
    {
        try
        {
            var id = await subscriptionService.CreateAsync(dto);
            return Ok(new { id });
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("{id:guid}/schedule")]
    public async Task<IActionResult> GetSchedule(Guid id)
    {
        try
        {
            var schedule = await subscriptionService.GetScheduleAsync(id);
            return Ok(schedule);
        }
        catch (ValidationException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSubscriptionDto dto)
    {
        try
        {
            var updatedId = await subscriptionService.UpdateAsync(id, dto);
            return Ok(new { id = updatedId });
        }
        catch (ValidationException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }
}