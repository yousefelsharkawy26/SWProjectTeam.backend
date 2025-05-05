using Models;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DentalManagementSystem.Services.Interfaces;
using Models.Responses;
using DataAccess.Repository.IRepository;
using Models.Requests;

namespace DentalManagementSystem.Services;
public class UserServices : IUserServices
{
    private readonly UserManager<User> _userManager;
    private readonly IEmailServices _emailServices;
    private readonly IUnitOfWork _unitOfWork;

    public UserServices(UserManager<User> userManager,
                        IUnitOfWork unitOfWork,
                        IEmailServices emailServices)
    {
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _emailServices = emailServices;
    }

    public async Task<User> CreateUser(User user)
    {
        string password = GenerateTemporaryPassword();
        user.UserName = await GenerateRandomUserName();
        await _userManager.CreateAsync(user, password);
        user = await _userManager.FindByEmailAsync(user.Email);
        await _userManager.AddToRoleAsync(user, "user");

        try
        {
            //if (user.FirstName != null)
            //    await _emailServices.SendConfirmationEmail(user.Email, password, user.FirstName);
            //else
            //    await _emailServices.SendConfirmationEmail(user.Email, password, user.Email.Substring(0, user.Email.IndexOf('@')));

        }
        catch (Exception ex)
        {
            throw new SmtpException($"Send Mail error: {ex.Message}");
        }

        return user;
    }

    public IEnumerable<UserResponse> GetUserByName(string userName)
    {
        return _unitOfWork.User.SearchUsersByName(userName);
    }

    public async Task<UserResponse> GetUserDetails(string userId)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);

        var userDetails = new UserResponse()
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Phone = user.PhoneNumber,
            ImageUrl = user.ImageUrl != null ? user.ImageUrl : "avatar.png",
            Bio = user.Bio,
            Permission = user.Permission,
            Id = user.Id
        };

        return userDetails;
    }
    public async Task<bool> UpdateUser(string userId, UserResponse userDetails)
    {
        var user = _userManager.Users.FirstOrDefault(u => u.Id == userId);

        user.FirstName = userDetails.FirstName;
        user.LastName = userDetails.LastName;
        user.Email = userDetails.Email;
        user.PhoneNumber = userDetails.Phone;
        user.Bio = userDetails.Bio;
        user.PhoneNumber = userDetails.Phone;

        var result = await _userManager.UpdateAsync(user);

        return result.Succeeded;
    }

    private string GenerateTemporaryPassword()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789@#";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, 8)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    public async Task<string> GenerateRandomUserName()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        Random rand = new Random();
        var username = new string(Enumerable.Repeat(chars, 8)
        .Select(s => s[rand.Next(s.Length)]).ToArray());

        if (await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == username) != null)
            username = await GenerateRandomUserName();

        return username;
    }

    public IEnumerable<UserResponse> FindUsersByEmailOrUserName(string search)
    {
        var users = _unitOfWork.User.FindUsersByEmailOrUserName(search);

        return users;
    }

    public User PatientRequestToUser(PatientRequest patient)
    {
        // In future add validation
        var dt = patient.DateOfBirth.Value;
        return new()
        {
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            Email = patient.Email,
            DateOfBirth = new DateOnly(dt.Year, dt.Month, dt.Day),
            Gender = patient.Gender,
            PhoneNumber = patient.Phone,
        };
    }
}
