using Models;
using Utilities;
using System.Text;
using Models.Requests;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using DentalManagementSystem.Services.Interfaces;

namespace DentalManagementSystem.Services;
public class AuthServices: IAuthServices
{
    private readonly IOptions<AppSettings> _appSettings;
    private readonly IUserServices _userServices;
    private readonly UserManager<User> _userManager;
    public AuthServices(UserManager<User> userManager,
                        IOptions<AppSettings> appSettings,
                        IUserServices userServices)
    {
        _userManager = userManager;
        _appSettings = appSettings;
        _userServices = userServices;
    }
    public IEnumerable<Claim> GetClaims(HttpRequest request)
    {
        var authHeader = request.Headers["Authorization"].FirstOrDefault();
        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            return null;// Unauthorized Invalid Token

        var token = authHeader.Substring("Bearer ".Length).Trim();
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.Value.SecretKey);

        try
        {
            // 3. Check Token is Valid
            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            // 4. Read Token (Claims)
            var jwtToken = (JwtSecurityToken)validatedToken;

            return jwtToken.Claims;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Token Error " + ex.Message);
            return null;
        }
    }
    public async Task<string> LoginService(LoginRequest loginUser)
    {
        var user = await _userManager.FindByEmailAsync(loginUser.Email);
        if (user != null && await _userManager.CheckPasswordAsync(user, loginUser.Password))
        {
            var roles = await _userManager.GetRolesAsync(user);
            var signKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Value.SecretKey));
            var expiresDate = DateTime.UtcNow.AddDays(7);

            var claims = new ClaimsIdentity(new Claim[] {
                new Claim("userId", user.Id),
                new Claim(ClaimTypes.Role, roles.First()),
                new Claim("expires", expiresDate.ToString())
                });

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = claims,
                Expires = expiresDate,
                SigningCredentials = new SigningCredentials(signKey, SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            var token = tokenHandler.WriteToken(securityToken);
            return token;
        }

        return null;
    }
    public async Task<bool> RegisterService(RegisterRequest registerUser)
    {
        if (registerUser.Role == Utilities.Utility.Admin_Role 
         || registerUser.Role == Utilities.Utility.Staff_Role)
            return false;

        if (!IsValidUser(registerUser))
            return false;

        var user = new User()
        {
            FirstName = registerUser.FirstName,
            LastName = registerUser.LastName,
            Email = registerUser.Email,
            UserName = await _userServices.GenerateRandomUserName(),
            Permission = registerUser.Role,
            Gender = registerUser.Gender
        };

        IdentityResult result = new();
        try
        {
            result = await _userManager.CreateAsync(user, registerUser.Password);
            await _userManager.AddToRoleAsync(user, registerUser.Role);
        }
        catch (Exception ex)
        {
            throw new TypeAccessException($"Error in ({nameof(RegisterService)}) {ex.Message }");
        }
        return result.Succeeded;
    }

    private bool IsValidUser(RegisterRequest registerUser)
    {
        if (registerUser == null)
            return false;

        if (string.IsNullOrEmpty(registerUser.FirstName) ||
            string.IsNullOrEmpty(registerUser.LastName) ||
            string.IsNullOrEmpty(registerUser.Email) ||
            string.IsNullOrEmpty(registerUser.Password) ||
            string.IsNullOrEmpty(registerUser.Gender))
            return false;

        return true;
    }

    public async Task<bool> ChangePassword(PasswordRequest password, HttpRequest token)
    {
        var userId = GetClaims(token).First(x => x.Type == "userId").Value;

        try
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            var result = await _userManager.ChangePasswordAsync(user, password.Password, password.NewPassword);

            return result.Succeeded;
        }
        catch (Exception ex)
        {
            throw new SecurityTokenException(ex.Message);
        }
    }
}
