using Business;
using DataAccess;

namespace Dispatcher;

public class Worker : BackgroundService
{
    private readonly List<Task> _receivers;
    private readonly ILogger<Worker> _logger;
    private readonly IServiceProvider _serviceProvider;

    public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _receivers = new List<Task>();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await using var scope = _serviceProvider.CreateAsyncScope();

            var configurationRepository = scope.ServiceProvider.GetRequiredService<IConfigurationRepository>();
            var receiver = scope.ServiceProvider.GetRequiredService<IReceiver>();
            var configs = await configurationRepository.GetConfigurationAsync();

            foreach (var config in configs)
            {
                _receivers.Add(receiver.StartAsync(config.Consumer, config.Publishers, stoppingToken));
            }

            await Task.WhenAll(_receivers);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
        }
    }
}