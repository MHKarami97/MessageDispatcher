using DataAccess.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess.Extensions
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
        {
            _ = services.AddSqlServer<ApplicationDbContext>(configuration.GetConnectionString("Default"));
            _ = services.AddTransient<IConfigurationRepository, ConfigurationRepository>();

            return services;
        }
    }
}