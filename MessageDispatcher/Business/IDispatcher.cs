using Util.Queue.Publisher;

namespace Business
{
    public interface IDispatcher
    {
        Task StartAsync(QueuePublisherConfig config);
        void Stop();
        Task DispatchAsync(string message);
    }
}