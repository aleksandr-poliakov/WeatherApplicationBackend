using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeatherForecast.Models;

public class MatchDay
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public DateOnly Date { get; set; }

    public bool IsPlayFree { get; set; } = false;

    [Required]
    public Guid SubscriptionId { get; set; }

    [ForeignKey(nameof(SubscriptionId))]
    public Subscription Subscription { get; set; } = null!;

    public ICollection<Match> Matches { get; set; } = [];
}