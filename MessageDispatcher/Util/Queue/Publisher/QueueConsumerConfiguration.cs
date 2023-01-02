using Util.Common;

namespace Util.Queue.Publisher
{
    public class QueuePublisherConfig
    {
        public ICollection<Endpoint> HostNames { get; set; }
        public string Exchange { get; set; }
        public string RouteingKey { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool AutomaticRecoveryEnabled { get; set; }
        public bool HasReProcess { get; set; }
    }
}