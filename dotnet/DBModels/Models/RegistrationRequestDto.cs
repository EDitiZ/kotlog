using System.ComponentModel.DataAnnotations;

namespace finance_backend.DBModels.Models;

public class RegistrationRequestDto
{
    [Required]
    public string UserName { get; set; } = string.Empty;
    [Required]
    public string FirstName { get; set; } = string.Empty;
    [Required]
    public string LastName { get; set; } = string.Empty;
    [Required]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string? Password { get; set; }
}