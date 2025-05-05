namespace DentalManagementSystem.Services.Interfaces;

public interface IEmailServices
{
    Task SendConfirmationEmail(string email, string password,string name);
}
