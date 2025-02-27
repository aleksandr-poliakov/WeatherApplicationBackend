using Microsoft.AspNetCore.Mvc;
using WeatherForecast.Dto;
using WeatherForecast.Models;
using WeatherForecast.Services;

namespace WeatherForecast.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(UserService service) : ControllerBase {
    [HttpGet]
    public async Task<IActionResult> GetAllUsers() {
        var users = await service.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpGet("{email}")]
    public async Task<ActionResult<UserResponseDto>> GetUser(string email)
    {
        var user = await service.GetUserByEmailAsync(email);
        if (user == null) return NotFound();
        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<UserResponseDto>> CreateUser([FromBody] UserCreateDto userDto)
    {
        var userResponse = await service.CreateUserAsync(userDto);
        return CreatedAtAction(nameof(GetUser), new { email = userResponse.Email }, userResponse);
    }
}
