using finance_backend.DBModels;
using finance_backend.DBModels.Models;

namespace finance_backend.Repository.Interfaces;

public interface IUsersRepository
{
    Task<Users> CreateAsync(Users user);
    Task<LoginResponse> LoginAsync(LoginRequestDto requestDto);
    bool IsUniqueUser(string username);
    bool IsUniqueEmail(string email);
    Task<Users> GetUserByIdAsync(long id);
    Task<Users> GetUserByEmailAsync(string email);
}