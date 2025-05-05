using Repository;
using DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using DataAccess.Context;

namespace DentalManagementSystem.Extensions;
public static class EFCoreExtensions
{
    public static IServiceCollection InjectDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("DevConn")));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
