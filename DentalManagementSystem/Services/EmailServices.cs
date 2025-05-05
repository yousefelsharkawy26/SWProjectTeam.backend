using DentalManagementSystem.Services.Interfaces;
using Utilities;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace DentalManagementSystem.Services;
public class EmailServices : IEmailServices
{
    private readonly IOptions<AppSettings> _appSettings;

    public EmailServices(IOptions<AppSettings> appSettings)
    {
        _appSettings = appSettings;
    }

    public async Task SendConfirmationEmail(string email, string password,string name)
    {
        var subject = "Complete Your Dental Clinic Registration";
        var body = $@"
            <html>
                <body>
                    <h2>Hello {name},</h2>
                    <p>You have been added to Our Dental Clinic.</p>
                    <p>your email: {email}</p>
                    <p>your password: {password}
                </body>
            </html>";

        await SendEmailAsync(email, subject, body);
    }

    private async Task SendEmailAsync(string to, string subject, string body)
    {
        var clint = new SmtpClient("smtp.gmail.com", 587)
        {
            Credentials = new NetworkCredential(_appSettings.Value.CompanyMail, _appSettings.Value.CompanyPassword),
            EnableSsl = true
        };
        try
        {
            await clint.SendMailAsync(_appSettings.Value.CompanyMail, to, subject, body);
            clint.Dispose();
        }
        catch (Exception ex)
        {
            throw new SmtpException($"SMTP Error: {ex.Message}");
        }
    }
}
