using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeatherForecast.Models;

public class User(string name, string email)
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }  

    [Required]
    [MaxLength(100)]
    public string Name { get; init; } = name;

    [Required, EmailAddress]
    [MaxLength(255)]
    public string Email { get; init; } = email;
}