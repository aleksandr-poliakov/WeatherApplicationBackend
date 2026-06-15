namespace WeatherForecast.Dto;

public class MatchDayDto
{
    public DateOnly Date { get; set; }
    public List<SinglesMatchDto> Singles { get; set; } = [];
    public List<DoublesMatchDto> Doubles { get; set; } = [];
    public List<string> Spielfrei { get; set; } = [];
}