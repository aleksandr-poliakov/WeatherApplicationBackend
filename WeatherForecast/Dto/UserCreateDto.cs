namespace WeatherForecast.Dto;

public class UserCreateDto : IUserDto {
    public required string Name { get; set; }
    public required string Email { get; set; }
}