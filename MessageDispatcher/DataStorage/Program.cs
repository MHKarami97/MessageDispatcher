using DataStorage;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services => { _ = services.AddHostedService<Worker>(); })
    .Build();

await host.RunAsync();