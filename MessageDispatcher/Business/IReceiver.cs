using Util.Queue.Consumer;
using Util.Queue.Publisher;

namespace Business
{
    public interface IReceiver
    {
        Task StartAsync(QueueConsumerConfig consumerConfig, ICollection<QueuePublisherConfig> publisherConfigs, CancellationToken cancellationToken);
    }
}