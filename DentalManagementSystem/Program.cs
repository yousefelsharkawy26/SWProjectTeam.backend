using DentalManagementSystem.Extensions;
using DentalManagementSystem.Controllers;
using DentalManagementSystem.SignalRHubs;

namespace DentalManagementSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddSwaggerExplorer()
                            .InjectDbContext(builder.Configuration)
                            .AddAppConfigure(builder.Configuration)
                            .AddApplicationServices()
                            .AddIdentityHandlersAndStores()
                            .ConfigureIdentityOptions()
                            .AddIdentityAuth(builder.Configuration)
                            .AddSignalR();

            var app = builder.Build();

            app.AddStaticFilesConfig(builder.Environment)
               .ConfigureSwaggerExplorer()
               .ConfigureCORS(builder.Configuration)
               .AddIdentityAuthMiddleWares();

            app.UseWebSockets();

            app.MapHub<NotificationHub>("/notificationHub");
            app.MapControllers();

            app.MapGroup("/api")
               .MapIdentityUserEndpoint();

            app.Run();
        }
    }
}
