namespace WeatherForecast.Dto;

public class UserResponseDto
{
    public required int Id { get; init; }
    public required string Name { get; set; }
    public required string Email { get; set; }
}
