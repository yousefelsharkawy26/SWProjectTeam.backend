using Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DentalManagementSystem.Services.Interfaces;

namespace DentalManagementSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IAuthServices _authServices;
    private readonly IFileServices _fileServices;
    private readonly IUserServices _userServices;
    private readonly INotificationServices _notificationServices;
    private readonly UserManager<User> _userManager;
    public UsersController(IAuthServices authServices,
                        IFileServices fileServices,
                        IUserServices userServices,
                        UserManager<User> userManager,
                        INotificationServices notificationServices)
    {
        _authServices = authServices;
        _fileServices = fileServices;
        _userManager = userManager;
        _userServices = userServices;
        _notificationServices = notificationServices;
    }

    [HttpGet]
    public IActionResult UserProfile()
    {
        try
        {
            var jwtToken = _authServices.GetClaims(Request);
            var userId = jwtToken.First(x => x.Type == "userId").Value;

            var user = _userServices.GetUserDetails(userId);

            return Ok(user);
        }
        catch (Exception ex)
        {
            return Unauthorized(new { message = "Invalid Token", error = ex.Message });
        }
    }

    [HttpGet("search")]
    public IActionResult SearchUsers(string name)
    {
        try
        {
            var users = _userServices.GetUserByName(name);

            return Ok(users);
        }
        catch
        {
            return BadRequest();
        }
    }

    [HttpPost("changeImage")]
    public async Task<IActionResult> ChangeUserImage(IFormFile File)
    {
        try
        {
            if (File == null)
                return BadRequest(new { message = "File is null" });

            var claims = _authServices.GetClaims(Request);
            var userId = claims.First(x => x.Type == "userId").Value;

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);

            string fileName = _fileServices.UploudFile(File, user.ImageUrl);
            user.ImageUrl = fileName;

            var res = await _userManager.UpdateAsync(user);
            if (res.Succeeded)
                return Ok(new { user.ImageUrl });
            return BadRequest();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("remove-user-image")]
    public async Task<IActionResult> RemoveImage(string name)
    {
        try
        {
            var userId = _authServices
                    .GetClaims(Request)
                    .First(x => x.Type == "userId").Value;
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);

            bool result = _fileServices.RemoveFile(name);

            if (result)
                return Ok(new { message = "success" });
            return BadRequest(new { message = "error" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("users-search")]
    public IActionResult GetUserILikeUsernameOrEmail(string search)
    {
        try
        {
            var users = _userServices
                .FindUsersByEmailOrUserName(search);

            if (users == null)
                return NotFound(new { message = "No users found" });

            return Ok(users);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("user-notifications")]
    public IActionResult GetUserNotifications()
    {
        try
        {
            var userId = _authServices
                .GetClaims(Request)
                .First(x=>x.Type=="userId").Value;

            var notifications = _notificationServices.GetUserNotifications(userId);

            return Ok(notifications);
        }
        catch (Exception ex) { return BadRequest(new {message = ex.Message}); }
    }
}
