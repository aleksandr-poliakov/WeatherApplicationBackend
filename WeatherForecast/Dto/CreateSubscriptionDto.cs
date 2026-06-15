namespace WeatherForecast.Dto;

public class CreateSubscriptionDto
{
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public DayOfWeek MatchDayOfWeek { get; set; }
    public int SinglesMatchCount { get; set; }
    public int DoublesMatchCount { get; set; }
    public List<string> PlayerNames { get; set; } = [];
}