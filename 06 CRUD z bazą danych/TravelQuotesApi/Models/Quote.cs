using System.ComponentModel.DataAnnotations;

namespace TravelQuotesApi.Models;

public sealed class Quote
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Author { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string Message { get; set; } = string.Empty;
}