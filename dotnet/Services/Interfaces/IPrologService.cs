using finance_backend.DBModels.Models;

namespace finance_backend.Services.Interfaces;

public interface IPrologService
{
    ApiResponse ExecutePrologQuery(string query);
    ApiResponse GetBudgetsExecutePrologQuery(string query);

}