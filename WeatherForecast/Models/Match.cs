using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeatherForecast.Models;

public enum MatchType { Singles, Doubles }

public class Match
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public MatchType Type { get; set; }

    [Required]
    public Guid MatchDayId { get; set; }

    [ForeignKey(nameof(MatchDayId))]
    public MatchDay MatchDay { get; set; } = null!;

    public ICollection<MatchParticipant> Participants { get; set; } = [];
}