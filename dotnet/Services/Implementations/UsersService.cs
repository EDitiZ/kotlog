using System.Net;
using finance_backend.DBModels;
using finance_backend.DBModels.Models;
using finance_backend.Repository.Interfaces;
using finance_backend.Services.Interfaces;

namespace finance_backend.Services.Implementations;

public class UsersService : IUserService
{
    private readonly IUsersRepository _usersRepository;
    private ApiResponse _apiResponse;
    private readonly string? _secretKey;

    public UsersService(IUsersRepository usersRepository, IConfiguration configuration)
    {
        _usersRepository = usersRepository;
        _apiResponse = new ApiResponse();
        _apiResponse.ErrorMessages = new List<string>();
        _secretKey = configuration.GetValue<string>("ApiSettings:Secret");
    }
    
    public async Task<ApiResponse> RegisterAsync(RegistrationRequestDto requestDto)
    {
        bool isUserNameUnique = _usersRepository.IsUniqueUser(requestDto.UserName);
        if (!isUserNameUnique)
        {
            _apiResponse.StatusCode = HttpStatusCode.BadRequest;
            _apiResponse.IsSuccess = false;
            _apiResponse.ErrorMessages?.Add("Username already exists!");
            return _apiResponse;
        }
        bool isUserEmailUnique = _usersRepository.IsUniqueEmail(requestDto.Email);
        if (!isUserEmailUnique)
        {
            _apiResponse.StatusCode = HttpStatusCode.BadRequest;
            _apiResponse.IsSuccess = false;
            _apiResponse.ErrorMessages?.Add("Email already exists!");
            return _apiResponse;
        }
        
        string hashedPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(requestDto.Password);
        requestDto.Password = hashedPassword;

        var user = new Users
        {
            FirstName = requestDto.FirstName,
            LastName = requestDto.LastName,
            Email = requestDto.Email,
            Password = requestDto.Password,
            UserName = requestDto.UserName
        };

        await _usersRepository.CreateAsync(user);

        _apiResponse.StatusCode = HttpStatusCode.Created;
        return _apiResponse;
    }

    public async Task<ApiResponse> Login(LoginRequestDto requestDto)
    {
        try
        {
            if (requestDto.UserName == "" || requestDto.Password == "")
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.ErrorMessages?.Add("Field/s can not be empty!");
                return _apiResponse;
            }

            var loginResponse = await _usersRepository.LoginAsync(requestDto);
            if (loginResponse.User == null || string.IsNullOrEmpty(loginResponse.AccessToken))
            {
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages?.Add("Username or password is incorrect!");
                return _apiResponse;
            }
      
            bool isPasswordCorrect = BCrypt.Net.BCrypt.EnhancedVerify(requestDto.Password, loginResponse.User.Password);
        
            if (!isPasswordCorrect)
            {
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages?.Add("Username or password is incorrect!");
                return _apiResponse;
            }
            _apiResponse.StatusCode = HttpStatusCode.OK;
            _apiResponse.Result = loginResponse;
            return _apiResponse;
        }
        catch (Exception e)
        {
            _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
            _apiResponse.IsSuccess = false;
            _apiResponse.ErrorMessages?.Add(e.ToString());
            return _apiResponse;
        }
    }
}