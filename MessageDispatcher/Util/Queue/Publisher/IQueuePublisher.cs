namespace Util.Queue.Publisher
{
    public interface IQueuePublisher
    {
        Task StartAsync(QueuePublisherConfig config);
        void Stop();
        void Add(string message);
    }
}