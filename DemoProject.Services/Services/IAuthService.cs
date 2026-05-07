using DemoProject.Core.Models.Dto;

namespace DemoProject.Core.Services;

public interface IAuthService
{
    LoginResponse? Login(LoginRequest request);
}
