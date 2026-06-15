using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeatherForecast.Models;

public class Player
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = "";

    [Required]
    public Guid SubscriptionId { get; set; }

    [ForeignKey(nameof(SubscriptionId))]
    public Subscription Subscription { get; set; } = null!;

    public ICollection<MatchParticipant> Participations { get; set; } = [];
    public ICollection<PlayerMatchDayAbsence> Absences { get; set; } = [];
}