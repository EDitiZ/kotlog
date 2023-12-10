using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using finance_backend.Data;
using finance_backend.DBModels;
using finance_backend.DBModels.Models;
using finance_backend.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace finance_backend.Repository.Implementations;

public class UserRepository : IUsersRepository
{
    private readonly ApplicationDbContext _db;
    private readonly string? _secretKey;

    public UserRepository(ApplicationDbContext dbContext, IConfiguration configuration)
    {
        _db = dbContext;
        _secretKey = configuration.GetValue<string>("ApiSettings:Secret");
    }
    
    public async Task<Users> CreateAsync(Users user)
    {
        await _db.Users.AddAsync(user);
        await _db.SaveChangesAsync();
        user.Password = "";
        return user;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequestDto requestDto)
    {
        var user = await _db.Users.FirstOrDefaultAsync(U => U.UserName.ToLower() == requestDto.UserName.ToLower());
        if (user == null)
        {
            return new LoginResponse()
            {
                User = null,
                AccessToken = ""
            };
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_secretKey ?? throw new InvalidOperationException());

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Id.ToString()),
            }),
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        LoginResponse loginResponse = new LoginResponse()
        {
            User = user,
            AccessToken = tokenHandler.WriteToken(token)
        };
        return loginResponse;
    }

    public bool IsUniqueUser(string username)
    {
        if (_db.Users.FirstOrDefault(u => u.UserName.ToLower() == username.ToLower()) != null)
        {
            return false;
        }
        return true;
    }

    public bool IsUniqueEmail(string email)
    {
        if (_db.Users.FirstOrDefault(u => u.Email.ToLower() == email.ToLower()) != null)
        {
            return false;
        }
        return true;
    }

    public async Task<Users> GetUserByIdAsync(long id)
    {
        return await _db.Users.AsNoTracking().FirstOrDefaultAsync(U => U.Id == id);
    }

    public async Task<Users> GetUserByEmailAsync(string email)
    {
        return await _db.Users.AsNoTracking().FirstOrDefaultAsync(U => U.Email.ToLower() == email.ToLower());
    }
}