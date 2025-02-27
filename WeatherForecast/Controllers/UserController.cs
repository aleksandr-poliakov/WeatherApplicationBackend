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

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserResponseDTO>> GetUser(int id) {
        var user = await service.GetUserByIdAsync(id);
        if (user == null) return NotFound();
        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<UserResponseDTO>> CreateUser([FromBody] UserCreateDTO userDto)
    {
        var user = await service.CreateUserAsync(userDto);
        return CreatedAtAction(nameof(GetUser), new { email = user.Email }, new UserResponseDTO { Name = user.Name, Email = user.Email });
    }
}
