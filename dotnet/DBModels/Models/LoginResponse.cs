namespace finance_backend.DBModels.Models;

public class LoginResponse
{
    public Users? User { get; set; }
    public string? AccessToken { get; set; }
}