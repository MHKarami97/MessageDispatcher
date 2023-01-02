using Util.Queue.Consumer;
using Util.Queue.Publisher;

namespace DataAccess
{
    public class Configuration
    {
        public QueueConsumerConfig Consumer { get; set; }
        public ICollection<QueuePublisherConfig> Publishers { get; set; }
    }
}