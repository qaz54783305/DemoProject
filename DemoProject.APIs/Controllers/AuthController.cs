using DemoProject.Core.Models;
using DemoProject.Core.Models.Dto;
using DemoProject.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace DemoProject.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService) => _authService = authService;

    /// <summary>登入取得 JWT Token</summary>
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var result = _authService.Login(request);
        return result == null
            ? Unauthorized(ApiResponse<string>.Fail("帳號或密碼錯誤"))
            : Ok(ApiResponse<LoginResponse>.Ok(result));
    }
}
