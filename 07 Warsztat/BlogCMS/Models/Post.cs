using System.ComponentModel.DataAnnotations;

namespace BlogCMS.Models;

public sealed class Post
{
    public int Id { get; set; }

    [Required]
    [MaxLength(150)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(5000)]
    public string Content { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string ImageUrl { get; set; } = string.Empty;

    public DateTime Published { get; set; }
}