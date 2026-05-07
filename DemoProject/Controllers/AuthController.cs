using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DemoProject.Data;
using DemoProject.Models;
using DemoProject.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace DemoProject.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _config;

    public AuthController(AppDbContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    /// <summary>登入取得 JWT Token（測試帳號：admin / Admin@123）</summary>
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var hash = HashPassword(request.Password);
        var user = _db.Users.FirstOrDefault(u => u.Username == request.Username && u.PasswordHash == hash);

        if (user == null)
            return Unauthorized(ApiResponse<string>.Fail("帳號或密碼錯誤"));

        var expiry = DateTime.UtcNow.AddMinutes(_config.GetValue<int>("Jwt:ExpiryMinutes", 60));
        var token = GenerateToken(user.Username, user.Role, expiry);

        return Ok(ApiResponse<LoginResponse>.Ok(new LoginResponse
        {
            Token = token,
            Username = user.Username,
            Role = user.Role,
            Expiration = expiry
        }));
    }

    private string GenerateToken(string username, string role, DateTime expiry)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, role)
        };
        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: expiry,
            signingCredentials: creds
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static string HashPassword(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }
}
