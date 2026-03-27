using Domain.Services.Users;
using Microsoft.AspNetCore.Mvc;

namespace FinalProjASP.Net.Controllers;
[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly UsersService _service;

    public AuthController(UsersService service)
    {
        _service = service;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserRequest request)
    {
        var result = await _service.Register(request);
        return Ok(result);
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserRequest request)
    {
        var result = await _service.Login(request);
        return Ok(result);
    }
    [HttpPost("admin")]
    public async Task<IActionResult> CreateAdmin(CreateAdminRequest request)
    {
        var result = await _service.CreateAdmin(request);
        return Ok(result);
    }
}