using System.ComponentModel.DataAnnotations;

namespace WeatherWorkerService.Models;

public sealed class Weather
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string City { get; set; } = string.Empty;

    [Required]
    [MaxLength(10)]
    public string Country { get; set; } = string.Empty;

    public double TemperatureC { get; set; }

    public double FeelsLikeC { get; set; }

    public int Humidity { get; set; }

    public int Pressure { get; set; }

    public double WindSpeed { get; set; }

    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}