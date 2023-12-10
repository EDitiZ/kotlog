using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace finance_backend.DBModels;


public class Users
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; init; }
    [MaxLength(12)]
    public string UserName { get; set; } = string.Empty;
    [MaxLength(25)]
    public string FirstName { get; set; } = string.Empty;
    [MaxLength(25)]
    public string LastName { get; set; } = string.Empty;
    [MaxLength(25)]
    public string Email { get; set; } = string.Empty;
    public string? Password { get; set; }
}