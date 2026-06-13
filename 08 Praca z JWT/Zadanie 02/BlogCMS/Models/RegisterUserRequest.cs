using System.ComponentModel.DataAnnotations;

namespace BlogCMS.Models;

public sealed class RegisterUserRequest
{
    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}