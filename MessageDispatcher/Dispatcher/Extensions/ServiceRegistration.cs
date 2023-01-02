using Business.Extensions;
using DataAccess.Extensions;
using Util.Extensions;

namespace Dispatcher.Extensions
{
    public static class ServiceRegistration
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            _ = services.AddUtil();
            _ = services.AddDataAccess(configuration);
            _ = services.AddBusinessServices();

            return services;
        }
    }
}