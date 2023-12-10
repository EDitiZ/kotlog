namespace finance_backend.DBModels.Models;

public class TransactionResponse
{
    public string Category { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Date { get; set; } = string.Empty;
}