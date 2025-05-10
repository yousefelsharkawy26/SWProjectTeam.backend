using DentalManagementSystem.Services;
using DentalManagementSystem.Services.Interfaces;

namespace DentalManagementSystem.Extensions;

public static class AppServicesCollection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthServices, AuthServices>();
        services.AddScoped<IClinicServices, ClinicServices>();
        services.AddScoped<IDentistServices, DentistServices>();
        services.AddScoped<IEmailServices, EmailServices>();
        services.AddScoped<IFileServices, FileServices>();
        services.AddScoped<IPhoneServices, PhoneServices>();
        services.AddScoped<IUserServices, UserServices>();
        services.AddScoped<IPostServices, PostServices>();
        services.AddScoped<INotificationServices, NotificationServices>();
        services.AddScoped<IInventoryServices, InventoryServices>();

        return services;
    }
}
