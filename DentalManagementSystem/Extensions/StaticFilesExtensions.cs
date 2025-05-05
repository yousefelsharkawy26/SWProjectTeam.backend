using Microsoft.Extensions.FileProviders;

namespace DentalManagementSystem.Extensions;
public static class StaticFilesExtensions
{
    public static WebApplication AddStaticFilesConfig(this WebApplication app, IWebHostEnvironment env)
    {
        // Enable displaying browser links.
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(Path.Combine(env.WebRootPath, "images")),
            RequestPath = "/images"
        });

        return app;
    }
}
