using System.Diagnostics;
using System.Text.RegularExpressions;
using finance_backend.DBModels.Models;
using finance_backend.Services.Interfaces;

namespace finance_backend.Services.Implementations;

public class PrologService : IPrologService
{
    private const string PrologFilePath = "/home/diti/Desktop/finances.pl";
    private ApiResponse _apiResponse;

    public PrologService()
    {
        _apiResponse = new ApiResponse();
    }
    public ApiResponse ExecutePrologQuery(string query)
    {
        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = "prolog",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            using (var process = new Process{ StartInfo = psi})
            {
                process.Start();
                using (var writer = process.StandardInput)
                {
                    if (writer.BaseStream.CanWrite)
                    {
                        writer.WriteLine($"['{PrologFilePath}'].");
                        writer.WriteLine(query);
                        writer.Close();
                    }
                }

                using (var reader = process.StandardOutput)
                {
                    var response = reader.ReadToEnd();
                    var matches = Regex.Matches(response, @"{[\s\S]+?}");

                    var transactions = new List<TransactionResponse>();
                    foreach (Match match in matches)
                    {
                        var transaction = Newtonsoft.Json.JsonConvert.DeserializeObject<TransactionResponse>(match.Value);
                        transactions.Add(transaction);
                    }

                    _apiResponse.Result = transactions;
                    return _apiResponse;
                }
            }
        }
        catch (Exception e)
        {
            return new ApiResponse { ErrorMessages = new List<string>() };
        }
    }
    
    
    public ApiResponse GetBudgetsExecutePrologQuery(string query)
    {
        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = "prolog",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            using (var process = new Process{ StartInfo = psi})
            {
                process.Start();
                using (var writer = process.StandardInput)
                {
                    if (writer.BaseStream.CanWrite)
                    {
                        writer.WriteLine($"['{PrologFilePath}'].");
                        writer.WriteLine(query);
                        writer.Close();
                    }
                }

                using (var reader = process.StandardOutput)
                {
                    var response = reader.ReadToEnd();
                    var matches = Regex.Matches(response, @"{[\s\S]+?}");

                    var budgets = new List<BudgetsResponse>();
                    foreach (Match match in matches)
                    {
                        var budget = Newtonsoft.Json.JsonConvert.DeserializeObject<BudgetsResponse>(match.Value);
                        budgets.Add(budget);
                    }

                    _apiResponse.Result = budgets;
                    return _apiResponse;
                }
            }
        }
        catch (Exception e)
        {
            return new ApiResponse { ErrorMessages = new List<string>() };
        }
    }
}