using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeatherForecast.Models;

public class User {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }  // Primary Key

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required, EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;
    
    // ✅ Конструктор для удобного создания объекта
    public User(string name, string email)
    {
        Name = name;
        Email = email;
    }
}