using System.ComponentModel.DataAnnotations;

namespace finance_backend.DBModels.Models;

public class LoginRequestDto
{
    [Required]
    public string UserName { get; set; } = string.Empty;
    [Required]
    public string? Password { get; set; }

}