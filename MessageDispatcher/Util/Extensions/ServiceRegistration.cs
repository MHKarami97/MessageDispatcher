using Microsoft.Extensions.DependencyInjection;
using Util.Queue.Consumer;
using Util.Queue.Publisher;

namespace Util.Extensions
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddUtil(this IServiceCollection services)
        {
            _ = services.AddTransient<IQueueConsumer, QueueConsumer>();
            _ = services.AddTransient<IQueuePublisher, QueuePublisher>();
            _ = services.AddSingleton<ITime, Time>();

            return services;
        }
    }
}