using finance_backend.DBModels.Models;
using finance_backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace finance_backend.Controllers;

[ApiController]
[Route("/api/finances")]
public class PrologController : ControllerBase
{
    private readonly IPrologService _prologService;
    private ApiResponse _apiResponse;

    public PrologController(IPrologService prologService)
    {
        _prologService = prologService;
        _apiResponse = new ApiResponse();
    }

    [HttpPost("add-transaction")]
    public IActionResult AddTransaction([FromBody] TransactionRequestDto request)
    {
        string prologQuery = $"add_transaction('{request.User}', '{request.Category}', '{request.Amount}', '{request.Date}').";
        Console.WriteLine($"Prolog Query: {prologQuery}");  

        _apiResponse = _prologService.ExecutePrologQuery(prologQuery);
        Console.WriteLine($"Prolog Response: {_apiResponse}");  

        return Ok(_apiResponse);
    }
    
    [HttpGet("show-transactions/{user}")]
    public IActionResult ShowTransactions(string user)
    {
        string prologQuery = $"inquire_financial_status('{user}').";
        _apiResponse = _prologService.ExecutePrologQuery(prologQuery);

        return Ok(_apiResponse);
    }
    
    [HttpPost("set-budget-goal")]
    public IActionResult SetBudgetGoal([FromBody] BudgetGoalRequestDto request)
    {
        string prologQuery = $"set_budget_goal('{request.User}', '{request.Category}', {request.Budget}).";
        _apiResponse = _prologService.ExecutePrologQuery(prologQuery);

        return Ok(_apiResponse);
    }
    
    [HttpGet("all-transactions/{user}")]
    public IActionResult AllTransactions(string user)
    {
        string prologQuery = $"show_transactions_json('{user}').";
        _apiResponse = _prologService.ExecutePrologQuery(prologQuery);

        return Ok(_apiResponse);
    }

    [HttpGet("all-budgets")]
    public IActionResult AllBudgets(string user)
    {
        string prologQuery = $"show_transactions_budget('{user}').";
        _apiResponse = _prologService.GetBudgetsExecutePrologQuery(prologQuery);

        return Ok(_apiResponse);
    }

    [HttpPost(template: "reset-user")]
    public IActionResult ResetUser(string user)
    {
        string prologQuery = $"remove_user_data('{user}').";
        _apiResponse = _prologService.ExecutePrologQuery(prologQuery);
        
        return Ok(_apiResponse);
    }

}