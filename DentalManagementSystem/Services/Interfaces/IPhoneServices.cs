namespace DentalManagementSystem.Services.Interfaces;
public interface IPhoneServices
{
    Task SendSms(string phoneNumber, string message);
}
