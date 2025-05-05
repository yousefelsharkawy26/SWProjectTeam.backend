using Models;
using Models.Requests;
using Models.Responses;

namespace DentalManagementSystem.Services.Interfaces;
public interface IUserServices
{
    Task<User> CreateUser(User user);
    Task<UserResponse> GetUserDetails(string userId);
    Task<bool> UpdateUser(string userId, UserResponse userDetails);
    IEnumerable<UserResponse> GetUserByName(string userName);
    Task<string> GenerateRandomUserName();
    IEnumerable<UserResponse> FindUsersByEmailOrUserName(string search);
    User PatientRequestToUser(PatientRequest patient);
}
