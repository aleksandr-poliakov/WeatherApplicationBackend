using System.ComponentModel.DataAnnotations;

namespace WeatherForecast.Models;

public class Subscription
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public DateOnly StartDate { get; set; }

    [Required]
    public DateOnly EndDate { get; set; }

    [Required]
    public DayOfWeek MatchDayOfWeek { get; set; }

    [Required]
    public int SinglesMatchCount { get; set; }

    [Required]
    public int DoublesMatchCount { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Player> Players { get; set; } = [];
    public ICollection<MatchDay> MatchDays { get; set; } = [];
}