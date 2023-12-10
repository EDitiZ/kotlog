using finance_backend.DBModels.Models;

namespace finance_backend.Services.Interfaces;

public interface IUserService
{
    Task<ApiResponse> RegisterAsync(RegistrationRequestDto requestDto);
    Task<ApiResponse> Login(LoginRequestDto requestDto);
}