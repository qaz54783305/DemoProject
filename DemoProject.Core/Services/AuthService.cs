using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DemoProject.Core.Data;
using DemoProject.Core.Models.Dto;
using DemoProject.Core.Settings;
using Microsoft.IdentityModel.Tokens;

namespace DemoProject.Core.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _db;
    private readonly JwtSettings _jwt;

    public AuthService(AppDbContext db, JwtSettings jwt)
    {
        _db = db;
        _jwt = jwt;
    }

    public LoginResponse? Login(LoginRequest request)
    {
        var hash = HashPassword(request.Password);
        var user = _db.Users.FirstOrDefault(u => u.Username == request.Username && u.PasswordHash == hash);
        if (user == null) return null;

        var expiry = DateTime.UtcNow.AddMinutes(_jwt.ExpiryMinutes);
        var token = GenerateToken(user.Username, user.Role, expiry);

        return new LoginResponse
        {
            Token = token,
            Username = user.Username,
            Role = user.Role,
            Expiration = expiry
        };
    }

    private string GenerateToken(string username, string role, DateTime expiry)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, role)
        };
        var token = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: expiry,
            signingCredentials: creds
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public static string HashPassword(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }
}
