namespace Dispatcher.Extensions
{
    public static class SerilogConfiguration
    {
        public static IHostBuilder ConfigureLogger(this IHostBuilder builder)
        {
            _ = builder.ConfigureLogging((context, loggingBuilder) => { _ = loggingBuilder.ClearProviders(); });

            return builder;
        }
    }
}