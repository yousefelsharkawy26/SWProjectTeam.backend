using Models.Requests;
using System.Security.Claims;

namespace DentalManagementSystem.Services.Interfaces;
public interface IAuthServices
{
    IEnumerable<Claim> GetClaims(HttpRequest request);
    Task<string> LoginService(LoginRequest loginUser);
    Task<bool> RegisterService(RegisterRequest registerUser);
    Task<bool> ChangePassword(PasswordRequest password, HttpRequest token);
}
