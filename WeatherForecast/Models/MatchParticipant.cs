using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeatherForecast.Models;

public class MatchParticipant
{
    [Required]
    public Guid MatchId { get; set; }

    [ForeignKey(nameof(MatchId))]
    public Match Match { get; set; } = null!;

    [Required]
    public Guid PlayerId { get; set; }

    [ForeignKey(nameof(PlayerId))]
    public Player Player { get; set; } = null!;
}