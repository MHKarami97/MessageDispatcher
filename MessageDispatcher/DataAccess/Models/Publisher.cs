using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models
{
    public class Publisher
    {
        [Key]
        public int Id { get; set; }
        public string HostNames { get; set; } = string.Empty;
        public string Exchange { get; set; } = string.Empty;
        public string RouteingKey { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool AutomaticRecoveryEnabled { get; set; }
        public bool HasReProcess { get; set; }
    }
}
