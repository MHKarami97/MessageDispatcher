namespace Util.Queue.Consumer
{
    public interface IQueueConsumer : IDisposable
    {
        public event ReceivedEventHandler Received;
        Task StartAsync(QueueConsumerConfig config);
        void Stop();
    }
}