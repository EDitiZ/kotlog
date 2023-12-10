using finance_backend.DBModels.Models;
using finance_backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace finance_backend.Controllers;

[ApiController]
[Route("/api/users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private ApiResponse _apiResponse;

    public UsersController(IUserService userService)
    {
        _userService = userService;
        _apiResponse = new ApiResponse();
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Register([FromBody] RegistrationRequestDto requestDto)
    {
        _apiResponse = await _userService.RegisterAsync(requestDto);
        return Ok(_apiResponse);
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
    {
        _apiResponse = await _userService.Login(loginRequestDto);
        return Ok(_apiResponse);
    }
}