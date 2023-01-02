using Util.Common;

namespace Util.Queue.Consumer
{
    public class QueueConsumerConfig
    {
        public ICollection<Endpoint> HostNames { get; set; }
        public DeadLetter DeadLetter { get; set; }
        public string Exchange { get; set; }
        public string ExchangeType { get; set; }
        public bool ExchangeAutoDelete { get; set; }
        public bool Durable { get; set; }
        public string RouteingKey { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Queue { get; set; }
        public int RequeueMessageRetryCount { get; set; }
        public bool AutomaticRecoveryEnabled { get; set; }
    }
    
    public class DeadLetter
    {
        public string Queue { get; set; }
        public string Exchange { get; set; }
        public string RoutingKey { get; set; }
    }
}