using Microsoft.Extensions.DependencyInjection;

namespace Business.Extensions
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            _ = services.AddTransient<IReceiver, Receiver>();
            _ = services.AddTransient<IDispatcher, Dispatcher>();

            return services;
        }
    }
}