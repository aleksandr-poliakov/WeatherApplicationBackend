namespace WeatherForecast.Dto;

public class UserResponseDto : IUserDto {
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
}