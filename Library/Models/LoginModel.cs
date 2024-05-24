using System.ComponentModel.DataAnnotations;

namespace Library.Models;

public class LoginModel
{
    [EmailAddress]
    [Required]
    public string? Email { get; set; }

    [Required]
    public string? Password { get; set; }
}