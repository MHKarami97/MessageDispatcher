using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models
{
    public class Consumer
    {
        [Key]
        public int Id { get; set; }
        public string HostNames { get; set; } = string.Empty;
        public string Exchange { get; set; } = string.Empty;
        public string ExchangeType { get; set; } = string.Empty;
        public bool ExchangeAutoDelete { get; set; }
        public bool Durable { get; set; }
        public string RouteingKey { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Queue { get; set; } = string.Empty;
        public int RequeueMessageRetryCount { get; set; }
        public bool AutomaticRecoveryEnabled { get; set; }
        public ICollection<Publisher> Publishers { get; set; } = new List<Publisher>();
    }
}
