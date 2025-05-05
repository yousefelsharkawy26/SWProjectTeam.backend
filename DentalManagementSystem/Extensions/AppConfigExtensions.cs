using Utilities;

namespace DentalManagementSystem.Extensions;
public static class AppConfigExtensions
{
    public static WebApplication ConfigureCORS(this WebApplication app, IConfiguration config)
    {
        // Configure the HTTP request pipeline.
        app.UseCors(c =>
            c.WithOrigins(config["FrontUrl"])
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());

        return app;
    }

    public static IServiceCollection AddAppConfigure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AppSettings>(configuration.GetSection("AppSettings"));

        return services;
    }
}
