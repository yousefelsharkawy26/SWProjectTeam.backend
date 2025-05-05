using Models.Requests;
using Models.Responses;
using Microsoft.AspNetCore.Authorization;
using DentalManagementSystem.Services.Interfaces;

namespace DentalManagementSystem.Controllers
{
    public static class IdentityUserEndpoints
    {
        public static IEndpointRouteBuilder MapIdentityUserEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("signin", SignIn);
            app.MapPost("signup", SignUp);
            app.MapGet("userdetails", UserDetails);
            app.MapPost("updateuser", UpdateUserDetails);
            app.MapPut("changepassword", ChangePassword);

            return app;
        }

        [AllowAnonymous]
        private static async Task<IResult> SignIn(LoginRequest loginUser, IAuthServices _authServices)
        {
            var token = await _authServices.LoginService(loginUser);
            if (token != null)
                return Results.Ok(new { token });
            return Results.BadRequest(new { message = "User Or Password is incorrect" });
        }
        [AllowAnonymous]
        private static async Task<IResult> SignUp(IAuthServices _authServices, RegisterRequest dto)
        {
            var result = await _authServices.RegisterService(dto);

            if (result)
            {
                var token = await _authServices.LoginService(new() { Email = dto.Email, Password = dto.Password });
                return Results.Ok(new { token });
            }
            return Results.BadRequest(new { message = "Register error" });
        }
        private static async Task<IResult> UserDetails(IAuthServices _authServices, IUserServices userServices
                                                     , HttpRequest Request)
        {
            try
            {
                var claims = _authServices.GetClaims(Request);
                var userId = claims.First(x => x.Type == "userId").Value;

                var userDetails = await userServices.GetUserDetails(userId);

                return Results.Ok(userDetails);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex);
            }
        }
        private static async Task<IResult> UpdateUserDetails(HttpRequest Request, IAuthServices _authServices
                                                           , IUserServices _userServices, UserResponse dto)
        {
            try
            {
                var claims = _authServices.GetClaims(Request);
                var userId = claims.First(x => x.Type == "userId").Value;

                var result = await _userServices.UpdateUser(userId, dto);
                if (result)
                    return Results.Ok("User updated successfully");
                return Results.BadRequest("Faild to update user data");
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex);
            }
        }
        private static async Task<IResult> ChangePassword(IAuthServices _authServices, HttpRequest Request, PasswordRequest dto)
        {
            try
            {
                var result = await _authServices.ChangePassword(dto, Request);
                
                if (!result)
                    return Results.BadRequest("Password change failed");

                return Results.Ok("Password updated successfully");
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex);
            }

        }
    }
}
