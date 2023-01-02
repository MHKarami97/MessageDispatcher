using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Util.Queue.Consumer;
using Util.Queue.Publisher;

namespace Business
{
    public class Receiver : IReceiver
    {
        private readonly IQueueConsumer _consumer;
        private readonly ILogger<Receiver> _logger;
        private readonly List<IDispatcher> _dispatchers;
        private readonly IServiceProvider _serviceProvider;

        public Receiver(ILogger<Receiver> logger, IQueueConsumer consumer, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _consumer = consumer;
            _serviceProvider = serviceProvider;
            _dispatchers = new List<IDispatcher>();
        }

        public async Task StartAsync(QueueConsumerConfig consumerConfig, ICollection<QueuePublisherConfig> publisherConfigs, CancellationToken cancellationToken)
        {
            try
            {
                await StartPublisherAsync(publisherConfigs.ToList());
                await StartConsumerAsync(consumerConfig);

                _consumer.Received += ProcessorAsync;

                await Task.Delay(-1, cancellationToken);

                Stop();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "error on StartAsync");
            }
        }

        private Task StartConsumerAsync(QueueConsumerConfig config)
        {
            return _consumer.StartAsync(config);
        }

        private async Task StartPublisherAsync(List<QueuePublisherConfig> publisherConfigs)
        {
            foreach (var config in publisherConfigs)
            {
                var dispatcher = _serviceProvider.GetRequiredService<IDispatcher>();
                await dispatcher.StartAsync(config);

                _dispatchers.Add(dispatcher);
            }

            await Task.CompletedTask;
        }

        private async Task<bool> ProcessorAsync(string message)
        {
            foreach (var dispatcher in _dispatchers)
            {
                await dispatcher.DispatchAsync(message);
            }

            return true;
        }

        private void Stop()
        {
            _consumer.Stop();

            foreach (var dispatcher in _dispatchers)
            {
                dispatcher.Stop();
            }
        }
    }
}