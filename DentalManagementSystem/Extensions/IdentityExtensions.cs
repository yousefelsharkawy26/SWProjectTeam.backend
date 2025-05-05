using Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DataAccess.Context;

namespace DentalManagementSystem.Extensions
{
    public static class IdentityExtensions
    {
        public static IServiceCollection AddIdentityHandlersAndStores(this IServiceCollection services)
        {
            services.AddIdentityApiEndpoints<User>()
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<AppDbContext>();
            return services;
        }

        public static IServiceCollection ConfigureIdentityOptions(this IServiceCollection services)
        {
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = true;
            });
            return services;
        }

        public static IServiceCollection AddIdentityAuth(this IServiceCollection services, IConfiguration config)
        {
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(y =>
                {
                    y.SaveToken = false;
                    y.TokenValidationParameters = new()
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(config["AppSettings:SecretKey"]!)),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });
            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build();

                //options.AddPolicy("HasLibraryId", policy => policy.RequireClaim("libraryId"));
                //options.AddPolicy("FemaleOnly", policy => policy.RequireClaim("gender", "Female"));
                //options.AddPolicy("Under10", policy => policy.RequireAssertion(context =>
                //Int32.Parse(context.User.Claims.First(x => (x.Type == "age")).Value) < 10
                //));
            });
            return services;
        }

        public static WebApplication AddIdentityAuthMiddleWares(this WebApplication app)
        {
            app.UseAuthentication();
            app.UseAuthorization();

            return app;
        }
    }
}
