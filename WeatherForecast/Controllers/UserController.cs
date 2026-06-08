using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeatherForecast.Dto;
using WeatherForecast.Services;

namespace WeatherForecast.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController(IUserService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<UserResponseDto>>> GetAllUsers()
    {
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
    [AllowAnonymous]
    public async Task<ActionResult<UserResponseDto>> CreateUser([FromBody] UserCreateDto userDto)
    {
        try
        {
            var userResponse = await service.CreateUserAsync(userDto);
            return CreatedAtAction(nameof(GetUser), new { email = userResponse.Email }, userResponse);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }
}
