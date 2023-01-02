using Dispatcher;
using Dispatcher.Extensions;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureLogger()
    .ConfigureMetric()
    .ConfigureServices((hostContext, services) =>
    {
        _ = services.RegisterServices(hostContext.Configuration);
        _ = services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();