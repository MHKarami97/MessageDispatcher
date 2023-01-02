using Util.Queue.Publisher;

namespace Business
{
    public class Dispatcher : IDispatcher
    {
        private readonly IQueuePublisher _publisher;

        public Dispatcher(IQueuePublisher publisher)
        {
            _publisher = publisher;
        }

        public async Task StartAsync(QueuePublisherConfig config)
        {
            await _publisher.StartAsync(config);
        }

        public Task DispatchAsync(string message)
        {
            _publisher.Add(message);

            return Task.CompletedTask;
        }

        public void Stop()
        {
            _publisher.Stop();
        }
    }
}