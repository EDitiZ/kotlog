namespace finance_backend.DBModels.Models;

public class TransactionRequestDto
{
    public string User { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Date { get; set; } = string.Empty;
}