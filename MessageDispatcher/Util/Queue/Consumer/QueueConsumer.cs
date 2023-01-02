using System.Text;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Util.Queue.Consumer
{
    public class QueueConsumer : IQueueConsumer
    {
        private IModel _channel;
        private IConnection _connection;
        private AsyncEventingBasicConsumer _consumer;
        private readonly ILogger<QueueConsumer> _logger;
        private const int WaitInReprocessOnMilliSecond = 100;
        private int _requeueMessageRetryCount;

        public event ReceivedEventHandler Received;

        public QueueConsumer(ILogger<QueueConsumer> logger)
        {
            _logger = logger;
        }

        public Task StartAsync(QueueConsumerConfig config)
        {
            _requeueMessageRetryCount = config.RequeueMessageRetryCount;

            var connectionFactory = new ConnectionFactory
            {
                UserName = config.UserName,
                Password = config.Password,
                AutomaticRecoveryEnabled = config.AutomaticRecoveryEnabled,
                DispatchConsumersAsync = true,
            };

            var endpoints = config.HostNames
                .Select(item => new AmqpTcpEndpoint(item.Name, item.Port))
                .ToList();

            _connection = connectionFactory.CreateConnection(endpoints);
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(
                config.Exchange,
                config.ExchangeType,
                config.Durable,
                config.ExchangeAutoDelete);

            _ = _channel.QueueDeclare(
                config.Queue,
                config.Durable,
                exclusive: false,
                autoDelete: false,
                arguments: new Dictionary<string, object>
                {
                    { "x-dead-letter-exchange", config.DeadLetter.Exchange },
                    { "x-dead-letter-routing-key", config.DeadLetter.RoutingKey }
                });

            _channel.QueueBind(
                config.Queue,
                config.Exchange,
                config.RouteingKey);

            #region DeadLetter

            // Dead Letter
            //if queue has 'x-dead-letter-routing-key' it can be 'direct', if not it must be 'fanout'
            _channel.ExchangeDeclare(
                config.DeadLetter.Exchange,
                "direct",
                durable: config.Durable,
                autoDelete: false);

            _ = _channel.QueueDeclare(
                config.DeadLetter.Queue,
                durable: true,
                exclusive: false,
                autoDelete: false);

            _channel.QueueBind(
                config.DeadLetter.Queue,
                config.DeadLetter.Exchange,
                config.DeadLetter.RoutingKey);

            #endregion

            _consumer = new AsyncEventingBasicConsumer(_channel);

            _consumer.Received += ReceivedData;

            _ = _channel.BasicConsume(config.Queue, autoAck: false, _consumer);

            return Task.CompletedTask;
        }

        public void Stop()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (_channel?.IsOpen == true)
            {
                _channel.BasicCancel(_consumer.ConsumerTags[0]);
            }

            _channel?.Dispose();
            _connection?.Dispose();
        }

        private async Task ReceivedData(object model, BasicDeliverEventArgs result)
        {
            try
            {
                var counter = 0;
                var message = Encoding.UTF8.GetString(result.Body.ToArray());

                while (counter < _requeueMessageRetryCount)
                {
                    var isSuccess = false;

                    try
                    {
                        isSuccess = await Received.Invoke(message);
                    }
                    catch (Exception)
                    {
                        // ignored
                    }

                    if (isSuccess)
                    {
                        _channel.BasicAck(result.DeliveryTag, multiple: false);

                        return;
                    }

                    counter++;
                    await Task.Delay(WaitInReprocessOnMilliSecond);
                }

                _channel.BasicReject(result.DeliveryTag, requeue: false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}