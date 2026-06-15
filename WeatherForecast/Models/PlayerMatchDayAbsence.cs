using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeatherForecast.Models;

public class PlayerMatchDayAbsence
{
    [Required]
    public Guid PlayerId { get; set; }

    [ForeignKey(nameof(PlayerId))]
    public Player Player { get; set; } = null!;

    [Required]
    public Guid MatchDayId { get; set; }

    [ForeignKey(nameof(MatchDayId))]
    public MatchDay MatchDay { get; set; } = null!;
}