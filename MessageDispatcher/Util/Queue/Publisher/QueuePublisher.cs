using System.Collections.Concurrent;
using System.Text;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Util.Queue.Publisher
{
    public class QueuePublisher : IQueuePublisher
    {
        private IModel _channel;
        private string _exchange;
        private bool _hasReprocess;
        private string _routingKey;
        private IBasicProperties _properties;
        private AutoResetEvent _newMessageHandler;
        private ConcurrentQueue<string> _internalMessageQueue;
        private readonly ILogger<QueuePublisher> _logger;

        public QueuePublisher(ILogger<QueuePublisher> logger)
        {
            _logger = logger;
        }

        public async Task StartAsync(QueuePublisherConfig config)
        {
            _hasReprocess = config.HasReProcess;

            _newMessageHandler = new AutoResetEvent(initialState: false);
            _internalMessageQueue = new ConcurrentQueue<string>();

            _exchange = config.Exchange;
            _routingKey = config.RouteingKey;

            var factory = new ConnectionFactory
            {
                UserName = config.UserName,
                Password = config.Password,
                AutomaticRecoveryEnabled = config.AutomaticRecoveryEnabled,
            };

            var endpoints = config.HostNames
                .Select(item => new AmqpTcpEndpoint(item.Name, item.Port))
                .ToList();

            var connection = factory.CreateConnection(endpoints);

            _channel = connection.CreateModel();

            _properties = _channel.CreateBasicProperties();

            await Task.Run(DoProcess);
        }

        public void Stop()
        {
        }

        public void Add(string message)
        {
            _internalMessageQueue.Enqueue(message);
            _ = _newMessageHandler.Set();
        }

        private async Task DoProcess()
        {
            while (true)
            {
                if (!_internalMessageQueue.IsEmpty)
                {
                    try
                    {
                        var messageDequeued = _internalMessageQueue.TryDequeue(out var message);

                        if (messageDequeued && message != null)
                        {
                            try
                            {
                                Send(message);
                            }
                            catch (Exception e)
                            {
                                _logger.LogCritical("Exception in QueuePublisher ", e);

                                if (_hasReprocess)
                                {
                                    await Task.Delay(20);

                                    Add(message);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogCritical("Exception in QueuePublisher", ex);
                    }
                }
                else
                {
                    _ = _newMessageHandler.WaitOne();
                }
            }
        }

        private void Send(string message)
        {
            _channel.BasicPublish(_exchange,
                _routingKey,
                _properties,
                Encoding.UTF8.GetBytes(message));
        }
    }
}