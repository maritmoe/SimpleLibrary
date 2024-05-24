using System.ComponentModel.DataAnnotations;

namespace Library.Models;
public class RegistrationModel
{

    [EmailAddress]
    [Required]
    public string? Email { get; set; }

    [Required]
    public string? Password { get; set; }

    [Required]
    public string? Name { get; set; }
}