namespace finance_backend.DBModels.Models;

public class BudgetGoalRequestDto
{
    public string User { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal Budget { get; set; }
}