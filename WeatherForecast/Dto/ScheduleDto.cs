namespace WeatherForecast.Dto;

public class ScheduleDto
{
    public Guid SubscriptionId { get; set; }
    public List<MatchDayDto> MatchDays { get; set; } = [];
}